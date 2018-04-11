using LccWebAPI.Constants;
using LccWebAPI.Database.Context;
using LccWebAPI.Models.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RiotSharp;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Interfaces;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LccWebAPI.Services
{
    public class MatchDataCollectionService : HostedService
    {
        private readonly IRiotApi _riotApi;
        private readonly IThrottledRequestHelper _throttledRequestHelper;
        private readonly IServiceProvider _serviceProvider;

        public MatchDataCollectionService(IRiotApi riotApi,
            IThrottledRequestHelper throttledRequestHelper,
            IServiceProvider serviceProvider)
        {
            _riotApi = riotApi;
            _throttledRequestHelper = throttledRequestHelper;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    using (var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                    {
                        try
                        {
                            var challengerPlayers = await _throttledRequestHelper.SendThrottledRequest(async () =>
                                await _riotApi.League.GetChallengerLeagueAsync(Region.euw, LeagueQueue.RankedSolo));
                            var mastersPlayers = await _throttledRequestHelper.SendThrottledRequest(async () =>
                                await _riotApi.League.GetMasterLeagueAsync(Region.euw, LeagueQueue.RankedSolo));
                            
                            foreach (var highEloPlayer in challengerPlayers.Entries.Concat(mastersPlayers.Entries))
                            {
                                try
                                {
                                    var riotSummoner = await _throttledRequestHelper.SendThrottledRequest(async () =>
                                        await _riotApi.Summoner.GetSummonerBySummonerIdAsync(Region.euw, long.Parse(highEloPlayer.PlayerOrTeamId)));

                                    var dbSummoner = await dbContext.Summoners.FirstOrDefaultAsync(x => x.AccountId == riotSummoner.AccountId);

                                    if (dbSummoner == null)
                                    {
                                        var newDbSummoner = new Models.Summoner.Summoner
                                        {
                                            SummonerId = riotSummoner.Id,
                                            AccountId = riotSummoner.AccountId,
                                            ProfileIconId = riotSummoner.ProfileIconId,
                                            Level = riotSummoner.Level,
                                            RevisionDate = riotSummoner.RevisionDate,
                                            SummonerName = riotSummoner.Name,
                                            LastUpdatedDate = new DateTime()
                                        };

                                        dbContext.Summoners.Add(newDbSummoner);
                                        await dbContext.SaveChangesAsync();

                                        dbSummoner = newDbSummoner;
                                    }

                                    // If the summoner has had updates post the date what we have on our records
                                    // RevisionDate can be anything from a level up to new games played
                                    if (riotSummoner.RevisionDate > dbSummoner.LastUpdatedDate)
                                    {
                                        /*
                                        * TODO: Update Summoner information
                                        *  I.E Level, name...etc
                                        */

                                        DateTime? collectionFromDate;
                                        DateTime? collectionToDate;

                                        if (dbSummoner.LastUpdatedDate == DateTime.MinValue)
                                        {
                                            collectionFromDate = null;
                                            collectionToDate = null;
                                        }
                                        else
                                        {
                                            collectionFromDate = dbSummoner.LastUpdatedDate;
                                            collectionToDate = DateTime.Now;
                                        }

                                        var riotMatchList = await _throttledRequestHelper.SendThrottledRequest(
                                            async () =>
                                                await _riotApi.Match.GetMatchListAsync(
                                                    Region.euw, 
                                                    riotSummoner.AccountId,
                                                    null,
                                                    null,
                                                    null,
                                                    collectionFromDate, //No filter if null on these dates
                                                    collectionToDate,
                                                    0, //starting index
                                                    25)); //ending index

                                        if (riotMatchList?.Matches != null)
                                        {
                                            foreach (var match in riotMatchList?.Matches)
                                            {
                                                if (!dbContext.Matches.Any(x => x.GameId == match.GameId))
                                                {
                                                    var newDbMatch = await ConvertRiotMatchReferenceToDbMatch(match);
                                                    if (newDbMatch != null)
                                                    {
                                                        dbContext.Matches.Add(newDbMatch);

                                                        var riotMatchTimeline = await _throttledRequestHelper.SendThrottledRequest(
                                                                async () => await _riotApi.Match.GetMatchTimelineAsync(Region.euw, newDbMatch.GameId));

                                                        if (riotMatchTimeline != null)
                                                        {
                                                            dbContext.MatchTimelines.Add(ConvertRiotMatchTimelineToDbMatchTimeline(riotMatchTimeline, newDbMatch.GameId));
                                                        }

                                                        await dbContext.SaveChangesAsync();
                                                    }
                                                }
                                            }
                                        }

                                        dbSummoner.LastUpdatedDate = riotSummoner.RevisionDate;
                                    }
                                }
                                catch (RiotSharpException)
                                {
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        catch (RiotSharpException)
                        {
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private async Task<Models.Match.Match> ConvertRiotMatchReferenceToDbMatch(MatchReference riotMatchReference)
        {
            try
            {
                var newDbMatch = new Models.Match.Match();

                var riotMatch = await _throttledRequestHelper.SendThrottledRequest(async () => await _riotApi.Match.GetMatchAsync(Region.euw, riotMatchReference.GameId));

                if (riotMatch != null)
                {
                    newDbMatch.GameId = riotMatch.GameId;
                    newDbMatch.GameDuration = riotMatch.GameDuration;
                    newDbMatch.GameDate = riotMatch.GameCreation;
                    newDbMatch.GamePatch = riotMatch.GameVersion;
                    newDbMatch.WinningTeamId = riotMatch.Participants.FirstOrDefault(x => x.Stats.Win)?.TeamId;

                    var winningTeamParticipants = riotMatch.Participants.Where(x => x.TeamId == newDbMatch.WinningTeamId);
                    var winningTeamParticipantIdentifies = riotMatch.ParticipantIdentities.Where(x => winningTeamParticipants.Any(y => y.ParticipantId == x.ParticipantId));
                    var winningTeamstats = riotMatch.Teams.FirstOrDefault(x => x.TeamId == newDbMatch.WinningTeamId);
                    
                    newDbMatch.Teams.Add(ConvertRiotParticipantsToDbTeam(winningTeamParticipantIdentifies, winningTeamParticipants, winningTeamstats));

                    var losingTeamParticipants = riotMatch.Participants.Where(x => x.TeamId != newDbMatch.WinningTeamId);
                    var losingTeamParticipantIdentifies = riotMatch.ParticipantIdentities.Where(x => losingTeamParticipants.Any(y => y.ParticipantId == x.ParticipantId));
                    var losingTeamStats = riotMatch.Teams.FirstOrDefault(x => x.TeamId != newDbMatch.WinningTeamId);

                    newDbMatch.Teams.Add(ConvertRiotParticipantsToDbTeam(losingTeamParticipantIdentifies, losingTeamParticipants, losingTeamStats));

                    return newDbMatch;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        private MatchTeam ConvertRiotParticipantsToDbTeam(IEnumerable<ParticipantIdentity> riotParticipantIdentities , IEnumerable<Participant> riotParticipants, TeamStats riotTeamStats)
        {
            var team = new MatchTeam
            {
                BaronKills = riotTeamStats.BaronKills,
                DragonKills = riotTeamStats.DragonKills,
                InhibitorKills = riotTeamStats.InhibitorKills,
                RiftHeraldKills = riotTeamStats.RiftHeraldKills,
                TeamId = riotTeamStats.TeamId
            };

            foreach (var participant in riotParticipants)
            {
                try
                {
                    var participantIdentity = riotParticipantIdentities.FirstOrDefault(x => x.ParticipantId == participant.ParticipantId).Player;

                    team.Players.Add
                    (
                        new MatchPlayer()
                        {
                            AccountId = participantIdentity.AccountId,
                            SummonerId = participantIdentity.SummonerId,
                            SummonerName = participantIdentity.SummonerName,
                            ProfileIconId = participantIdentity.ProfileIcon,
                            TeamId = participant.TeamId,
                            ParticipantId = participant.ParticipantId,
                            HighestAcheivedTierLastSeason = participant.HighestAchievedSeasonTier.ToString(),
                            Kills = participant.Stats.Kills,
                            Deaths = participant.Stats.Deaths,
                            Assists = participant.Stats.Assists,
                            ChampionId = participant.ChampionId,
                            ChampionLevel = participant.Stats.ChampLevel,

                            //Item ids
                            TrinketId = participant.Stats.Item6,
                            Item1Id = participant.Stats.Item0,
                            Item2Id = participant.Stats.Item1,
                            Item3Id = participant.Stats.Item2,
                            Item4Id = participant.Stats.Item3,
                            Item5Id = participant.Stats.Item4,
                            Item6Id = participant.Stats.Item5,

                            //Rune ids
                            PrimaryRuneStyleId = participant.Stats.PerkPrimaryStyle,
                            PrimaryRuneSubStyleOneId = participant.Stats.Perk0,
                            PrimaryRuneSubStyleTwoId = participant.Stats.Perk1,
                            PrimaryRuneSubStyleThreeId = participant.Stats.Perk2,
                            PrimaryRuneSubStyleFourId = participant.Stats.Perk3,

                            SecondaryRuneStyleId = participant.Stats.PerkSubStyle,
                            SecondaryRuneSubStyleOneId = participant.Stats.Perk4,
                            SecondaryRuneSubStyleTwoId = participant.Stats.Perk5,

                            //Summoners spell ids
                            SummonerSpellOneId = participant.Spell1Id,
                            SummonerSpellTwoId = participant.Spell2Id,

                            //Gold
                            GoldEarned = participant.Stats.GoldEarned,
                            GoldSpent = participant.Stats.GoldSpent,

                            //Vision
                            VisionScore = participant.Stats.VisionScore,
                            WardsPlaced = participant.Stats.WardsPlaced,
                            VisionWardsBoughtInGame = participant.Stats.VisionWardsBoughtInGame,
                            SightWardsBoughtInGame = participant.Stats.SightWardsBoughtInGame,

                            //Damage dealt/taken
                            TotalDamageTaken = participant.Stats.TotalDamageTaken,
                            TotalDamageDealt = participant.Stats.TotalDamageDealt,
                            TotalDamageDealtToChampions = participant.Stats.TotalDamageDealtToChampions,

                            TrueDamageTaken = participant.Stats.TrueDamageTaken,
                            TrueDamageDealt = participant.Stats.TrueDamageDealt,
                            TrueDamageDealtToChampions = participant.Stats.TrueDamageDealtToChampions,

                            MagicDamageTaken = participant.Stats.MagicalDamageTaken,
                            MagicDamageDealt = participant.Stats.MagicDamageDealt,
                            MagicDamageDealtToChampions = participant.Stats.MagicDamageDealtToChampions,

                            PhysicalDamageTaken = participant.Stats.PhysicalDamageTaken,
                            PhysicalDamageDealt = participant.Stats.PhysicalDamageDealt,
                            PhysicalDamageDealtToChampions = participant.Stats.PhysicalDamageDealtToChampions,

                            LargestCriticalStrike = participant.Stats.LargestCriticalStrike,

                            //objectives
                            FirstTowerAssist = participant.Stats.FirstTowerAssist,
                            FirstTowerKill = participant.Stats.FirstTowerKill,
                            TurretKills = participant.Stats.TurretKills,
                            DamageDealtToTurrets = participant.Stats.DamageDealtToTurrets,

                            FirstInhibitorAssist = participant.Stats.FirstInhibitorAssist,
                            FirstInhibitorKill = participant.Stats.FirstInhibitorKill,
                            InhibitorKills = participant.Stats.InhibitorKills,

                            DamageDealtToObjectives = participant.Stats.DamageDealtToObjectives,
                            ObjectivePlayerScore = participant.Stats.ObjectivePlayerScore,

                            //Kills
                            FirstBloodAssist = participant.Stats.FirstBloodAssist,
                            FirstBloodKill = participant.Stats.FirstBloodKill,

                            LargestMultiKill = participant.Stats.LargestMultiKill,
                            LargestKillingSpree = participant.Stats.LargestKillingSpree,
                            PentaKills = participant.Stats.PentaKills,
                            QuadraKills = participant.Stats.QuadraKills,
                            KillingSprees = participant.Stats.KillingSprees,
                            DoubleKills = participant.Stats.DoubleKills,

                            //Farming
                            NeutralMinionsKilled = participant.Stats.NeutralMinionsKilled,
                            NeutralMinionsKilledEnemyJungle = participant.Stats.NeutralMinionsKilledEnemyJungle,
                            NeutralMinionsKilledTeamJungle = participant.Stats.NeutralMinionsKilledTeamJungle,

                            //Misc info
                            TimeCCingOthers = participant.Stats.TimeCCingOthers,
                            TotalTimeCrowdControlDealt = participant.Stats.TotalTimeCrowdControlDealt,

                            TotalHeal = participant.Stats.TotalHeal,
                            TotalUnitsHealed = participant.Stats.TotalUnitsHealed,

                            TotalScoreRank = participant.Stats.TotalScoreRank
                        }
                    );
                }
                catch (Exception)
                {
                }
            }

            return team;
        }

        private MatchTimeline ConvertRiotMatchTimelineToDbMatchTimeline(Timeline riotMatchTimeline, long gameId)
        {
            return new MatchTimeline
            {
                GameId = gameId,
                Events = ConvertRiotMatchTimelineEventsToDbMatchTimelineEvents(riotMatchTimeline.Frames)
            };
        }

        private ICollection<MatchEvent> ConvertRiotMatchTimelineEventsToDbMatchTimelineEvents(IEnumerable<Frame> riotFrames)
        {
            var matchEvents = new List<MatchEvent>();

            foreach (var fr in riotFrames)
            {
                foreach (var ev in fr.Events)
                {
                    matchEvents.Add(new MatchEvent()
                    {
                        Type = ev.Type.ToString(),
                        Timestamp = ev.Timestamp,
                        ParticipantId = ev.ParticipantId,
                        ItemId = ev.ItemId,
                        SkillSlot = ev.SkillSlot,
                        LevelUpType = ev.LevelUpType,
                        WardType = ev.WardType,
                        CreatorId = ev.CreatorId,
                        KillerId = ev.KillerId,
                        VictimId = ev.VictimId,
                        AfterId = ev.AfterId,
                        BeforeId = ev.BeforeId,
                        TeamId = ev.TeamId,
                        BuildingType = ev.BuildingType,
                        LaneType = ev.LaneType,
                        TowerType = ev.TowerType,
                        MonsterType = ev.MonsterType,
                        MonsterSubType = ev.MonsterSubType
                    });
                }
            }

            return matchEvents;
        }
    }
}
