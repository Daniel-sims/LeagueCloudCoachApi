﻿using LccWebAPI.Constants;
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

        #region CachedInformation -> LccInformation
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
            
            matchupInformation.FriendlyTeam = CreateLccTeamInformationFromCache(match.FriendlyTeam);
            matchupInformation.EnemyTeam = CreateLccTeamInformationFromCache(match.EnemyTeam);

            return matchupInformation;
        }
        
        private LccTeamInformation CreateLccTeamInformationFromCache(Db_LccCachedTeamInformation cachedTeamInformation)
        {
            LccTeamInformation lccTeamInformation = new LccTeamInformation()
            {
                TotalKills = cachedTeamInformation.Players.Sum(x => x.Kills),
                TotalDeaths = cachedTeamInformation.Players.Sum(x => x.Deaths),
                TotalAssists = cachedTeamInformation.Players.Sum(x => x.Assists),
                DragonKills = cachedTeamInformation.DragonKills,
                BaronKills = cachedTeamInformation.BaronKills,
                RiftHeraldKills = cachedTeamInformation.RiftHeraldKills,
                InhibitorKills = cachedTeamInformation.InhibitorKills
            };

            foreach (Db_LccCachedPlayerStats player in cachedTeamInformation.Players)
            {
                lccTeamInformation.Players.Add(CreateLccPlayerStatsFromCache(player));
            }

            return lccTeamInformation;
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
            catch (Exception)
            {
                Console.WriteLine("Exception hit creating the player stats!");
            }

            return new LccPlayerStats();
        }
        #endregion

        #region Riot match information -> CachedInformation
        //Method triggered when a match is requested that we don't currently have in the cache
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

                cachedMatchupInformation.FriendlyTeam = await 
                    CreateCachedTeamInformationFromRiotMatch
                    (
                        match, 
                        match.Participants.Where(x => x.TeamId == usersTeamId).ToList(),
                        match.Participants.FirstOrDefault(x => x.ChampionId == usersChampionId).TeamId
                    );

                cachedMatchupInformation.EnemyTeam = await
                    CreateCachedTeamInformationFromRiotMatch
                    (
                        match,
                        match.Participants.Where(x => x.TeamId != usersTeamId).ToList(),
                        match.Participants.FirstOrDefault(x => x.ChampionId != usersChampionId).TeamId
                    );
                
                return cachedMatchupInformation;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when creating dbmodel for cached matchup info.");
            }

            return new Db_LccCachedCalculatedMatchupInfo();
        }
        
        private async Task<Db_LccCachedTeamInformation> CreateCachedTeamInformationFromRiotMatch(RiotSharp.Endpoints.MatchEndpoint.Match match, IList<Participant> teamParticipants, int teamId)
        {
            Db_LccCachedTeamInformation teamInformation = new Db_LccCachedTeamInformation
            {
                TotalKills = teamParticipants.Sum(x => x.Stats.Kills),
                TotalDeaths = teamParticipants.Sum(x => x.Stats.Kills),
                TotalAssists = teamParticipants.Sum(x => x.Stats.Kills),
                DragonKills = match.Teams.FirstOrDefault(x => x.TeamId == teamId).DragonKills,
                BaronKills = match.Teams.FirstOrDefault(x => x.TeamId == teamId).BaronKills,
                RiftHeraldKills = match.Teams.FirstOrDefault(x => x.TeamId == teamId).RiftHeraldKills,
                InhibitorKills = match.Teams.FirstOrDefault(x => x.TeamId == teamId).InhibitorKills
            };

            teamInformation.Players = new List<Db_LccCachedPlayerStats>();
            foreach (Participant participant in teamParticipants)
            {
                ParticipantIdentity participantIdentity = match.ParticipantIdentities.FirstOrDefault(x => x.ParticipantId == participant.ParticipantId);
                teamInformation.Players.Add(await 
                    CreateCachedPlayerStatsFromMatchupInfo
                    (
                        participantIdentity, 
                        participant
                    )
                );
            }

            return teamInformation;
        }

        private async Task<Db_LccCachedPlayerStats> CreateCachedPlayerStatsFromMatchupInfo(ParticipantIdentity participantIdentity, Participant participant)
        {
            try
            {
                List<LeaguePosition> leaguePosition = await _riotApi.League.GetLeaguePositionsAsync(Region.euw, participantIdentity.Player.SummonerId);
                LeaguePosition rankedSoloLeague = leaguePosition.FirstOrDefault(x => x.QueueType == LeagueQueue.RankedSolo);

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
                    ItemOne = CreateCachedItemInformation(Convert.ToInt32(participant.Stats.Item0)),
                    ItemTwo = CreateCachedItemInformation(Convert.ToInt32(participant.Stats.Item1)),
                    ItemThree = CreateCachedItemInformation(Convert.ToInt32(participant.Stats.Item2)),
                    ItemFour = CreateCachedItemInformation(Convert.ToInt32(participant.Stats.Item3)),
                    ItemFive = CreateCachedItemInformation(Convert.ToInt32(participant.Stats.Item4)),
                    ItemSix = CreateCachedItemInformation(Convert.ToInt32(participant.Stats.Item5)),
                    Trinket = CreateCachedItemInformation(Convert.ToInt32(participant.Stats.Item6)),
                    SummonerOne = CreateCachedSummonerSpellInformation(participant.Spell1Id),
                    SummonerTwo = CreateCachedSummonerSpellInformation(participant.Spell2Id),
                    Champion = CreateCachedChampionInformation(participant.ChampionId),
                    ChampionLevel = participant.Stats.ChampLevel,
                    PrimaryRuneStyle = CreateCachedRuneInformation(Convert.ToInt32(participant.Stats.PerkPrimaryStyle)),
                    PrimaryRuneSubOne = CreateCachedRuneInformation(Convert.ToInt32(participant.Stats.Perk0)),
                    PrimaryRuneSubTwo = CreateCachedRuneInformation(Convert.ToInt32(participant.Stats.Perk1)),
                    PrimaryRuneSubThree = CreateCachedRuneInformation(Convert.ToInt32(participant.Stats.Perk2)),
                    PrimaryRuneSubFour = CreateCachedRuneInformation(Convert.ToInt32(participant.Stats.Perk3)),
                    SecondaryRuneStyle = CreateCachedRuneInformation(Convert.ToInt32(participant.Stats.PerkSubStyle)),
                    SecondaryRuneSubOne = CreateCachedRuneInformation(Convert.ToInt32(participant.Stats.Perk4)),
                    SecondaryRuneSubTwo = CreateCachedRuneInformation(Convert.ToInt32(participant.Stats.Perk5))
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception thrown creating cahcedPlayerStatsFromMatchInfo : " + e.Message);
            }

            return new Db_LccCachedPlayerStats();
        }

        private Db_LccRune CreateCachedRuneInformation(int runeId)
        {
            List<Db_LccRune> runes = _runeStaticDataReposistory.GetAllRunes().ToList();

            return new Db_LccRune()
            {
                RuneId = runeId,
                RuneName = runes.FirstOrDefault(x => x.RuneId == runeId)?.RuneName,
                RunePathName = runes.FirstOrDefault(x => x.RuneId == runeId)?.RunePathName,
                Icon = runes.FirstOrDefault(x => x.RuneId == runeId)?.Icon,
                Key = runes.FirstOrDefault(x => x.RuneId == runeId)?.Key,
                ShortDesc = runes.FirstOrDefault(x => x.RuneId == runeId)?.ShortDesc,
                LongDesc = runes.FirstOrDefault(x => x.RuneId == runeId)?.LongDesc
            };
        }

        private Db_LccSummonerSpell CreateCachedSummonerSpellInformation(int spellId)
        {
            List<Db_LccSummonerSpell> summonerSpells = _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList();

            return new Db_LccSummonerSpell()
            {
                SummonerSpellId = spellId,
                SummonerSpellName = summonerSpells.FirstOrDefault(x => x.SummonerSpellId == spellId)?.SummonerSpellName,
                ImageFull = summonerSpells.FirstOrDefault(x => x.SummonerSpellId == spellId)?.ImageFull
            };
        }

        private Db_LccChampion CreateCachedChampionInformation(int championId)
        {
            List<Db_LccChampion> champions = _championStaticDataRepository.GetAllChampions().ToList();

            return new Db_LccChampion()
            {
                ChampionId = championId,
                ChampionName = champions.FirstOrDefault(x => x.ChampionId == championId)?.ChampionName,
                ImageFull = champions.FirstOrDefault(x => x.ChampionId == championId)?.ImageFull
            };
        }

        private Db_LccItem CreateCachedItemInformation(int itemId)
        {
            List<Db_LccItem> items = _itemStaticDataRepository.GetAllItems().ToList();

            return new Db_LccItem()
            {
                ItemId = itemId,
                ItemName = items?.FirstOrDefault(x => x.ItemId == itemId)?.ItemName,
                ImageFull = items?.FirstOrDefault(x => x.ItemId == itemId)?.ImageFull
            };
        }
        #endregion
    }
}
   