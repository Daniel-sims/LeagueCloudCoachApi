using LccWebAPI.Constants;
using LccWebAPI.Controllers.Models.Match;
using LccWebAPI.Controllers.Models.StaticData;
using LccWebAPI.Database.Models.Match;
using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Repository.Interfaces.StaticData;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Interfaces;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers.Utils.Match
{
    public class MatchControllerUtils : IMatchControllerUtils
    {
        private readonly IRiotApi _riotApi;

        private readonly IItemStaticDataRepository _itemStaticDataRepository;
        private readonly ISummonerSpellStaticDataRepository _summonerSpellStaticDataRepository;
        private readonly IChampionStaticDataRepository _championStaticDataRepository;
        private readonly IRunesStaticDataRepository _runeStaticDataReposistory;

        public MatchControllerUtils(
            IRiotApi riotApi,
            IItemStaticDataRepository itemStaticDataRepository,
            ISummonerSpellStaticDataRepository summonerSpellStaticDataRepository,
            IChampionStaticDataRepository championStaticDataRepository,
            IRunesStaticDataRepository runeStaticDataReposistory)
        {
            _riotApi = riotApi;

            _itemStaticDataRepository = itemStaticDataRepository;
            _summonerSpellStaticDataRepository = summonerSpellStaticDataRepository;
            _championStaticDataRepository = championStaticDataRepository;
            _runeStaticDataReposistory = runeStaticDataReposistory;
        }

        // Converts an instnace of Db_LccCachedCalculatedMatchupInfo into LccCalculatedMatchupInformation
        // These are essentially the same models but I don't want to be returning models straight from the db
        public LccCalculatedMatchupInformation CreateLccCalculatedMatchupInformationFromCache(Db_LccCachedCalculatedMatchupInfo match)
        {
            // General info
            LccCalculatedMatchupInformation matchupInformation = new LccCalculatedMatchupInformation()
            {
                MatchDate = match.MatchDate,
                MatchPatch = match.MatchPatch,
                MatchDuration = match.MatchDuration,
                GameId = match.GameId,
                FriendlyTeamWin = match.FriendlyTeamWin
            };

            // FRIENDLY TEAM INFORMATION
            LccTeamInformation friendlyTeamInformation = new LccTeamInformation
            {
                TotalKills = match.FriendlyTeam.Players.Sum(x => x.Kills),
                TotalDeaths = match.FriendlyTeam.Players.Sum(x => x.Deaths),
                TotalAssists = match.FriendlyTeam.Players.Sum(x => x.Assists),
                DragonKills = match.FriendlyTeam.DragonKills,
                BaronKills = match.FriendlyTeam.BaronKills,
                RiftHeraldKills = match.FriendlyTeam.RiftHeraldKills,
                InhibitorKills = match.FriendlyTeam.InhibitorKills
            };

            foreach (Db_LccCachedPlayerStats friendlyPlayer in match.FriendlyTeam.Players)
            {
                friendlyTeamInformation.Players.Add(CreateLccPlayerStatsFromCache(friendlyPlayer));
            }

            matchupInformation.FriendlyTeam = friendlyTeamInformation;

            // ENEMY TEAM INFORMATION
            LccTeamInformation enemyTeamInformation = new LccTeamInformation
            {
                TotalKills = match.EnemyTeam.Players.Sum(x => x.Kills),
                TotalDeaths = match.EnemyTeam.Players.Sum(x => x.Deaths),
                TotalAssists = match.EnemyTeam.Players.Sum(x => x.Assists),
                DragonKills = match.EnemyTeam.DragonKills,
                BaronKills = match.EnemyTeam.BaronKills,
                RiftHeraldKills = match.EnemyTeam.RiftHeraldKills,
                InhibitorKills = match.EnemyTeam.InhibitorKills
            };

            foreach (Db_LccCachedPlayerStats enemyPlayer in match.EnemyTeam.Players)
            {
                enemyTeamInformation.Players.Add(CreateLccPlayerStatsFromCache(enemyPlayer));
            }

            matchupInformation.EnemyTeam = enemyTeamInformation;

            return matchupInformation;
        }

        private LccPlayerStats CreateLccPlayerStatsFromCache(Db_LccCachedPlayerStats playerStats)
        {
            try
            {
                return new LccPlayerStats()
                {
                    SummonerName = playerStats.SummonerName,
                    Kills = playerStats.Kills,
                    Deaths = playerStats.Deaths,
                    Assists = playerStats.Assists,
                    MinionKills = playerStats.MinionKills,
                    RankedSoloDivision = playerStats?.RankedSoloDivision,
                    RankedSoloTier = playerStats?.RankedSoloTier,
                    RankedSoloLeaguePoints = playerStats?.RankedSoloLeaguePoints.ToString(),
                    RankedSoloWins = Convert.ToInt32(playerStats?.RankedSoloWins),
                    RankedSoloLosses = Convert.ToInt32(playerStats?.RankedSoloLosses),
                    ItemOne = new LccItemInformation()
                    {
                        ItemId = playerStats.ItemOne.ItemId,
                        ItemName = playerStats.ItemOne?.ItemName,
                        ImageFull = playerStats.ItemOne?.ImageFull
                    },
                    ItemTwo = new LccItemInformation()
                    {
                        ItemId = playerStats.ItemTwo.ItemId,
                        ItemName = playerStats.ItemTwo?.ItemName,
                        ImageFull = playerStats.ItemTwo?.ImageFull
                    },
                    ItemThree = new LccItemInformation()
                    {
                        ItemId = playerStats.ItemThree.ItemId,
                        ItemName = playerStats.ItemThree?.ItemName,
                        ImageFull = playerStats.ItemThree?.ImageFull
                    },
                    ItemFour = new LccItemInformation()
                    {
                        ItemId = playerStats.ItemFour.ItemId,
                        ItemName = playerStats.ItemFour?.ItemName,
                        ImageFull = playerStats.ItemFour?.ImageFull
                    },
                    ItemFive = new LccItemInformation()
                    {
                        ItemId = playerStats.ItemFive.ItemId,
                        ItemName = playerStats.ItemFive?.ItemName,
                        ImageFull = playerStats.ItemFive?.ImageFull
                    },
                    ItemSix = new LccItemInformation()
                    {
                        ItemId = playerStats.ItemSix.ItemId,
                        ItemName = playerStats.ItemSix?.ItemName,
                        ImageFull = playerStats.ItemSix?.ImageFull
                    },
                    Trinket = new LccItemInformation()
                    {
                        ItemId = playerStats.Trinket.ItemId,
                        ItemName = playerStats.Trinket?.ItemName,
                        ImageFull = playerStats.Trinket?.ImageFull
                    },
                    SummonerOne = new LccSummonerSpellInformation()
                    {
                        SummonerSpellId = playerStats.SummonerOne.SummonerSpellId,
                        SummonerSpellName = playerStats.SummonerOne?.SummonerSpellName,
                        ImageFull = playerStats.SummonerOne?.ImageFull
                    },
                    SummonerTwo = new LccSummonerSpellInformation()
                    {
                        SummonerSpellId = playerStats.SummonerTwo.SummonerSpellId,
                        SummonerSpellName = playerStats.SummonerTwo?.SummonerSpellName,
                        ImageFull = playerStats.SummonerTwo?.ImageFull
                    },
                    Champion = new LccChampionInformation()
                    {
                        ChampionId = playerStats.Champion.ChampionId,
                        ChampionName = playerStats.Champion?.ChampionName,
                        ImageFull = playerStats.Champion?.ImageFull
                    },
                    ChampionLevel = playerStats.ChampionLevel,
                    PrimaryRuneStyle = new LccRuneInformation()
                    {
                        RuneId = playerStats.PrimaryRuneStyle.RuneId,
                        RuneName = playerStats.PrimaryRuneStyle?.RuneName
                    },
                    PrimaryRuneSubOne = new LccRuneInformation()
                    {
                        RuneId = playerStats.PrimaryRuneSubOne.RuneId,
                        RuneName = playerStats.PrimaryRuneSubOne?.RuneName
                    },
                    PrimaryRuneSubTwo = new LccRuneInformation()
                    {
                        RuneId = playerStats.PrimaryRuneSubTwo.RuneId,
                        RuneName = playerStats.PrimaryRuneSubTwo?.RuneName
                    },
                    PrimaryRuneSubThree = new LccRuneInformation()
                    {
                        RuneId = playerStats.PrimaryRuneSubThree.RuneId,
                        RuneName = playerStats.PrimaryRuneSubThree?.RuneName
                    },
                    PrimaryRuneSubFour = new LccRuneInformation()
                    {
                        RuneId = playerStats.PrimaryRuneSubFour.RuneId,
                        RuneName = playerStats.PrimaryRuneSubFour?.RuneName
                    },
                    SecondaryRuneStyle = new LccRuneInformation()
                    {
                        RuneId = playerStats.SecondaryRuneStyle.RuneId,
                        RuneName = playerStats.SecondaryRuneStyle?.RuneName
                    },
                    SecondaryRuneSubOne = new LccRuneInformation()
                    {
                        RuneId = playerStats.SecondaryRuneSubOne.RuneId,
                        RuneName = playerStats.SecondaryRuneSubOne?.RuneName
                    },
                    SecondaryRuneSubTwo = new LccRuneInformation()
                    {
                        RuneId = playerStats.SecondaryRuneSubTwo.RuneId,
                        RuneName = playerStats.SecondaryRuneSubTwo?.RuneName
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception hit creating the player stats!");
            }

            return new LccPlayerStats();
        }


        // These two methods created a cached version to later use to lookup information
        private async Task<Db_LccCachedPlayerStats> CreateCachedPlayerStatsFromMatchupInfo(ParticipantIdentity participantIdentity, Participant participant)
        {
            try
            {
                List<LeaguePosition> leaguePosition = await _riotApi.League.GetLeaguePositionsAsync(Region.euw, participantIdentity.Player.SummonerId);
                LeaguePosition rankedSoloLeague = leaguePosition.FirstOrDefault(x => x.QueueType == LeagueQueue.RankedSolo);

                List<Db_LccItem> items = _itemStaticDataRepository.GetAllItems().ToList();
                List<Db_LccRune> runes = _runeStaticDataReposistory.GetAllRunes().ToList();
                List<Db_LccChampion> champions = _championStaticDataRepository.GetAllChampions().ToList();
                List<Db_LccSummonerSpell> summonerSpells = _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList();

                return new Db_LccCachedPlayerStats()
                {
                    SummonerId = participantIdentity.Player.SummonerId,
                    SummonerName = participantIdentity.Player.SummonerName,
                    Kills = participant.Stats.Kills,
                    Deaths = participant.Stats.Deaths,
                    Assists = participant.Stats.Assists,
                    MinionKills = participant.Stats.NeutralMinionsKilled + participant.Stats.TotalMinionsKilled,
                    RankedSoloDivision = rankedSoloLeague?.Rank,
                    RankedSoloTier = rankedSoloLeague?.Tier,
                    RankedSoloLeaguePoints = rankedSoloLeague?.LeaguePoints.ToString(),
                    RankedSoloWins = rankedSoloLeague.Wins,
                    RankedSoloLosses = rankedSoloLeague.Losses,
                    Trinket = new Db_LccItem()
                    {
                        ItemId = Convert.ToInt32(participant.Stats.Item6),
                        ItemName = items?.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item6))?.ItemName,
                        ImageFull = items?.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item6))?.ImageFull
                    },
                    ItemOne = new Db_LccItem()
                    {
                        ItemId = Convert.ToInt32(participant.Stats.Item0),
                        ItemName = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item0))?.ItemName,
                        ImageFull = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item0))?.ImageFull
                    },
                    ItemTwo = new Db_LccItem()
                    {
                        ItemId = Convert.ToInt32(participant.Stats.Item1),
                        ItemName = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item1))?.ItemName,
                        ImageFull = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item1))?.ImageFull
                    },
                    ItemThree = new Db_LccItem()
                    {
                        ItemId = Convert.ToInt32(participant.Stats.Item2),
                        ItemName = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item2))?.ItemName,
                        ImageFull = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item2))?.ImageFull
                    },
                    ItemFour = new Db_LccItem()
                    {
                        ItemId = Convert.ToInt32(participant.Stats.Item3),
                        ItemName = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item3))?.ItemName,
                        ImageFull = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item3))?.ImageFull
                    },
                    ItemFive = new Db_LccItem()
                    {
                        ItemId = Convert.ToInt32(participant.Stats.Item4),
                        ItemName = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item4))?.ItemName,
                        ImageFull = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item4))?.ImageFull
                    },
                    ItemSix = new Db_LccItem()
                    {
                        ItemId = Convert.ToInt32(participant.Stats.Item5),
                        ItemName = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item5))?.ItemName,
                        ImageFull = items.FirstOrDefault(x => x.ItemId == Convert.ToInt32(participant.Stats.Item5))?.ImageFull
                    },
                    SummonerOne = new Db_LccSummonerSpell()
                    {
                        SummonerSpellId = Convert.ToInt32(participant.Spell1Id),
                        SummonerSpellName = summonerSpells.FirstOrDefault(x => Convert.ToInt32(participant.Spell1Id) == x.SummonerSpellId)?.SummonerSpellName,
                        ImageFull = summonerSpells.FirstOrDefault(x => Convert.ToInt32(participant.Spell1Id) == x.SummonerSpellId)?.ImageFull
                    },
                    SummonerTwo = new Db_LccSummonerSpell()
                    {
                        SummonerSpellId = Convert.ToInt32(participant.Spell2Id),
                        SummonerSpellName = summonerSpells.FirstOrDefault(x => Convert.ToInt32(participant.Spell2Id) == x.SummonerSpellId)?.SummonerSpellName,
                        ImageFull = summonerSpells.FirstOrDefault(x => Convert.ToInt32(participant.Spell2Id) == x.SummonerSpellId)?.ImageFull
                    },
                    Champion = new Db_LccChampion()
                    {
                        ChampionId = participant.ChampionId,
                        ChampionName = champions.FirstOrDefault(x => x.ChampionId == participant.ChampionId)?.ChampionName,
                        ImageFull = champions.FirstOrDefault(x => x.ChampionId == participant.ChampionId)?.ImageFull
                    },
                    ChampionLevel = participant.Stats.ChampLevel,
                    PrimaryRuneStyle = new Db_LccRune()
                    {
                        RuneId = Convert.ToInt32(participant.Stats.PerkPrimaryStyle),
                        RuneName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkPrimaryStyle))?.RuneName,
                        RunePathName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkPrimaryStyle))?.RunePathName,
                        Icon = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkPrimaryStyle))?.Icon,
                        Key = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkPrimaryStyle))?.Key,
                        ShortDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkPrimaryStyle))?.ShortDesc,
                        LongDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkPrimaryStyle))?.LongDesc
                    },
                    PrimaryRuneSubOne = new Db_LccRune()
                    {
                        RuneId = Convert.ToInt32(participant.Stats.Perk0),
                        RuneName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk0))?.RuneName,
                        RunePathName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk0))?.RunePathName,
                        Icon = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk0))?.Icon,
                        Key = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk0))?.Key,
                        ShortDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk0))?.ShortDesc,
                        LongDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk0))?.LongDesc
                    },
                    PrimaryRuneSubTwo = new Db_LccRune()
                    {
                        RuneId = Convert.ToInt32(participant.Stats.Perk1),
                        RuneName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk1))?.RuneName,
                        RunePathName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk1))?.RunePathName,
                        Icon = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk1))?.Icon,
                        Key = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk1))?.Key,
                        ShortDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk1))?.ShortDesc,
                        LongDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk1))?.LongDesc
                    },
                    PrimaryRuneSubThree = new Db_LccRune()
                    {
                        RuneId = Convert.ToInt32(participant.Stats.Perk2),
                        RuneName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk2))?.RuneName,
                        RunePathName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk2))?.RunePathName,
                        Icon = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk2))?.Icon,
                        Key = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk2))?.Key,
                        ShortDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk2))?.ShortDesc,
                        LongDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk2))?.LongDesc
                    },
                    PrimaryRuneSubFour = new Db_LccRune()
                    {
                        RuneId = Convert.ToInt32(participant.Stats.Perk3),
                        RuneName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk3))?.RuneName,
                        RunePathName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk3))?.RunePathName,
                        Icon = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk3))?.Icon,
                        Key = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk3))?.Key,
                        ShortDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk3))?.ShortDesc,
                        LongDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk3))?.LongDesc
                    },
                    SecondaryRuneStyle = new Db_LccRune()
                    {
                        RuneId = Convert.ToInt32(participant.Stats.PerkSubStyle),
                        RuneName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkSubStyle))?.RuneName,
                        RunePathName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkSubStyle))?.RunePathName,
                        Icon = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkSubStyle))?.Icon,
                        Key = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkSubStyle))?.Key,
                        ShortDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkSubStyle))?.ShortDesc,
                        LongDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.PerkSubStyle))?.LongDesc
                    },
                    SecondaryRuneSubOne = new Db_LccRune()
                    {
                        RuneId = Convert.ToInt32(participant.Stats.Perk4),
                        RuneName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk4))?.RuneName,
                        RunePathName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk4))?.RunePathName,
                        Icon = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk4))?.Icon,
                        Key = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk4))?.Key,
                        ShortDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk4))?.ShortDesc,
                        LongDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk4))?.LongDesc
                    },
                    SecondaryRuneSubTwo = new Db_LccRune()
                    {
                        RuneId = Convert.ToInt32(participant.Stats.Perk5),
                        RuneName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk5))?.RuneName,
                        RunePathName = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk5))?.RunePathName,
                        Icon = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk5))?.Icon,
                        Key = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk5))?.Key,
                        ShortDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk5))?.ShortDesc,
                        LongDesc = runes.FirstOrDefault(x => x.RuneId == Convert.ToInt32(participant.Stats.Perk5))?.LongDesc
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception thrown creating cahcedPlayerStatsFromMatchInfo : " + e.Message);
            }

            return new Db_LccCachedPlayerStats();
        }

        public async Task<Db_LccCachedCalculatedMatchupInfo> CreateDatabaseModelForCalculatedMatchupInfo(RiotSharp.Endpoints.MatchEndpoint.Match match, long usersChampionId)
        {
            try
            {
                int usersTeamId = match.Participants.FirstOrDefault(x => x.ChampionId == usersChampionId).TeamId;

                IList<Participant> friendlyTeamParticipants = match.Participants.Where(x => x.TeamId == usersTeamId).ToList();
                IList<Participant> enemyTeamParticipants = match.Participants.Where(x => x.TeamId != usersTeamId).ToList();

                Db_LccCachedCalculatedMatchupInfo cachedMatchupInformation = new Db_LccCachedCalculatedMatchupInfo()
                {
                    GameId = match.GameId,
                    MatchDate = match.GameCreation,
                    MatchPatch = match.GameVersion,
                    MatchDuration = match.GameDuration,
                    FriendlyTeamWin = match.Teams.FirstOrDefault(x => x.TeamId == usersTeamId).Win == MatchOutcome.Win
                };

                cachedMatchupInformation.EnemyTeam = new Db_LccCachedTeamInformation()
                {
                    TotalKills = enemyTeamParticipants.Sum(x => x.Stats.Kills),
                    TotalDeaths = enemyTeamParticipants.Sum(x => x.Stats.Deaths),
                    TotalAssists = enemyTeamParticipants.Sum(x => x.Stats.Assists),
                    DragonKills = match.Teams.FirstOrDefault(x => x.TeamId != usersTeamId).DragonKills,
                    BaronKills = match.Teams.FirstOrDefault(x => x.TeamId != usersTeamId).BaronKills,
                    RiftHeraldKills = match.Teams.FirstOrDefault(x => x.TeamId != usersTeamId).RiftHeraldKills,
                    InhibitorKills = match.Teams.FirstOrDefault(x => x.TeamId != usersTeamId).InhibitorKills,
                };

                cachedMatchupInformation.FriendlyTeam = new Db_LccCachedTeamInformation()
                {
                    TotalKills = friendlyTeamParticipants.Sum(x => x.Stats.Kills),
                    TotalDeaths = friendlyTeamParticipants.Sum(x => x.Stats.Deaths),
                    TotalAssists = friendlyTeamParticipants.Sum(x => x.Stats.Assists),
                    DragonKills = match.Teams.FirstOrDefault(x => x.TeamId == usersTeamId).DragonKills,
                    BaronKills = match.Teams.FirstOrDefault(x => x.TeamId == usersTeamId).BaronKills,
                    RiftHeraldKills = match.Teams.FirstOrDefault(x => x.TeamId == usersTeamId).RiftHeraldKills,
                    InhibitorKills = match.Teams.FirstOrDefault(x => x.TeamId == usersTeamId).InhibitorKills,
                };

                cachedMatchupInformation.EnemyTeam.Players = new List<Db_LccCachedPlayerStats>();
                foreach (Participant enemyParticipant in enemyTeamParticipants)
                {
                    ParticipantIdentity enemyParticipanyIdentity = match.ParticipantIdentities.FirstOrDefault(x => x.ParticipantId == enemyParticipant.ParticipantId);
                    cachedMatchupInformation.EnemyTeam.Players.Add(await CreateCachedPlayerStatsFromMatchupInfo(enemyParticipanyIdentity, enemyParticipant));
                }

                cachedMatchupInformation.FriendlyTeam.Players = new List<Db_LccCachedPlayerStats>();
                foreach (Participant friendlyParticipant in friendlyTeamParticipants)
                {
                    ParticipantIdentity friendlyParticipanyIdentity = match.ParticipantIdentities.FirstOrDefault(x => x.ParticipantId == friendlyParticipant.ParticipantId);
                    cachedMatchupInformation.FriendlyTeam.Players.Add(await CreateCachedPlayerStatsFromMatchupInfo(friendlyParticipanyIdentity, friendlyParticipant));
                }

                return cachedMatchupInformation;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when creating dbmodel for cached matchup info.");
            }

            return new Db_LccCachedCalculatedMatchupInfo();
        }
    }
}
   
