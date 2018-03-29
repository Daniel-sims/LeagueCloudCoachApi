using LccWebAPI.Constants;
using LccWebAPI.Controllers.Models.Match;
using LccWebAPI.Controllers.Models.StaticData;
using LccWebAPI.Database.Models.Match;
using LccWebAPI.Database.Models.StaticData;
using LccWebAPI.Database.Repository.Interfaces.Match;
using LccWebAPI.Repository.Interfaces.Match;
using LccWebAPI.Repository.Interfaces.StaticData;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Interfaces;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IRiotApi _riotApi;

        private IBasicMatchupInformationRepository _matchupInformationRepository;
        private ICachedCalculatedMatchupInformationRepository _cachedCalculatedMatchupInformaton;

        private readonly IItemStaticDataRepository _itemStaticDataRepository;
        private readonly ISummonerSpellStaticDataRepository _summonerSpellStaticDataRepository;
        private readonly IChampionStaticDataRepository _championStaticDataRepository;
        private readonly IRunesStaticDataRepository _runeStaticDataReposistory;

        public MatchController(IRiotApi riotApi,
            IBasicMatchupInformationRepository matchupInformationRepository,
            ICachedCalculatedMatchupInformationRepository cachedCalculatedMatchupInformaton,
            IItemStaticDataRepository itemStaticDataRepository,
            ISummonerSpellStaticDataRepository summonerSpellStaticDataRepository,
            IChampionStaticDataRepository championStaticDataRepository,
            IRunesStaticDataRepository runeStaticDataReposistory)
        {
            _riotApi = riotApi;

            _matchupInformationRepository = matchupInformationRepository;
            _cachedCalculatedMatchupInformaton = cachedCalculatedMatchupInformaton;

            _itemStaticDataRepository = itemStaticDataRepository;
            _summonerSpellStaticDataRepository = summonerSpellStaticDataRepository;
            _championStaticDataRepository = championStaticDataRepository;
            _runeStaticDataReposistory = runeStaticDataReposistory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetMatchup")]
        public async Task<JsonResult> GetMatchup(long usersChampionId, string usersLane, long[] friendlyTeamChampions, long[] enemyTeamChampions, int maxMatchLimit = 5)
        {
            IList<Db_LccBasicMatchInfo> allMatchesInDatabase = _matchupInformationRepository.GetAllMatchups().ToList();
            IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            IList<long> enemyTeamChampionIds = enemyTeamChampions.ToList();

            var allMatchesContainingUsersChampion = allMatchesInDatabase.Where
                (x => x.LosingTeamChampions.Any(p => p.ChampionId == usersChampionId || x.WinningTeamChampions.Any(u => u.ChampionId == usersChampionId))).ToList();

            var allMatchesWithRequestedTeams =
                allMatchesContainingUsersChampion.Where
                //Check to see if the Enemys team Ids are the losing team, and the friendly team are the winning team
                (q => (enemyTeamChampionIds.All(e => q.LosingTeamChampions.Any(l => l.ChampionId == e))
                && friendlyTeamChampionIds.All(f => q.WinningTeamChampions.Any(l => l.ChampionId == f)))
                //Check to see if the winning team Ids are the losing team, and the enemy team are the winning team
                || (enemyTeamChampionIds.All(e => q.WinningTeamChampions.Any(l => l.ChampionId == e))
                && friendlyTeamChampionIds.All(f => q.LosingTeamChampions.Any(l => l.ChampionId == f))));

            List<LccCalculatedMatchupInformation> matchesToReturnToUser = new List<LccCalculatedMatchupInformation>();

            if (allMatchesWithRequestedTeams.Any())
            {
                int matchReturnCount = 0;

                foreach (var match in allMatchesWithRequestedTeams.OrderByDescending(x => x.MatchDate))
                {
                    if (matchReturnCount == maxMatchLimit)
                        break;

                    try
                    {
                        Db_LccCachedCalculatedMatchupInfo cachedMatchInfo = _cachedCalculatedMatchupInformaton.GetCalculatedMatchupInfoByGameId(match.GameId);

                        if (cachedMatchInfo == null)
                        {
                            Match riotMatchInformation = await _riotApi.Match.GetMatchAsync(Region.euw, match.GameId);
                            Db_LccCachedCalculatedMatchupInfo newCachedMatchInfo = CreateDatabaseModelForCalculatedMatchupInfo(riotMatchInformation, usersChampionId);

                            _cachedCalculatedMatchupInformaton.InsertCalculatedMatchupInfo(newCachedMatchInfo);
                            _cachedCalculatedMatchupInformaton.Save();

                            cachedMatchInfo = newCachedMatchInfo;
                        }

                        matchesToReturnToUser.Add(await CreateLccCalculatedMatchupInformationFromCache(cachedMatchInfo, usersChampionId));

                        matchReturnCount++;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Exception encountered when creating match :" + e.Message);
                    }
                }
            }
            
            return new JsonResult(matchesToReturnToUser);
        }

        // Converts an instnace of Db_LccCachedCalculatedMatchupInfo into LccCalculatedMatchupInformation
        // These are essentially the same models but I don't want to be returning models straight from the db
        private async Task<LccCalculatedMatchupInformation> CreateLccCalculatedMatchupInformationFromCache(Db_LccCachedCalculatedMatchupInfo match, long usersChampionId)
        {
            // General info
            LccCalculatedMatchupInformation matchupInformation = new LccCalculatedMatchupInformation()
            {
                MatchDate = match.MatchDate,
                MatchPatch = match.MatchPatch,
                GameId = match.GameId
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
                List<LeaguePosition> leaguePosition = await _riotApi.League.GetLeaguePositionsAsync(Region.euw, friendlyPlayer.SummonerId);

                friendlyTeamInformation.Players.Add
                    (
                        CreateLccPlayerStatsFromCache
                        (
                            friendlyPlayer,
                            leaguePosition.FirstOrDefault(x => x.QueueType == LeagueQueue.RankedSolo)
                        )
                    );
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
                List<LeaguePosition> leaguePosition = await _riotApi.League.GetLeaguePositionsAsync(Region.euw, enemyPlayer.SummonerId);

                enemyTeamInformation.Players.Add
                    (
                        CreateLccPlayerStatsFromCache
                        (
                            enemyPlayer,
                            leaguePosition.FirstOrDefault(x => x.QueueType == LeagueQueue.RankedSolo)
                        )
                    );
            }

            matchupInformation.EnemyTeam = enemyTeamInformation;
            return matchupInformation;
        }
        private LccPlayerStats CreateLccPlayerStatsFromCache(Db_LccCachedPlayerStats playerStats, LeaguePosition leaguePosition)
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
                    RankedSoloDivision = leaguePosition?.Rank,
                    RankedSoloTier = leaguePosition?.Tier,
                    RankedSoloLeaguePoints = leaguePosition?.LeaguePoints.ToString(),
                    RankedSoloWins = Convert.ToInt32(leaguePosition?.Wins),
                    RankedSoloLosses = Convert.ToInt32(leaguePosition?.Losses),
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
            catch(Exception e)
            {
                Console.WriteLine("Exception hit creating the player stats!");
            }

            return new LccPlayerStats();
        }


        // These two methods created a cached version to later use to lookup information
        private Db_LccCachedPlayerStats CreateCachedPlayerStatsFromMatchupInfo(ParticipantIdentity participantIdentity, Participant participant)
        {
            try
            {
                return new Db_LccCachedPlayerStats()
                {
                    SummonerId = 0,
                    SummonerName = "",
                    Kills = 0,
                    Deaths = 0,
                    Assists = 0,
                    MinionKills = 0,
                    RankedSoloDivision = "",
                    RankedSoloTier = "",
                    RankedSoloLeaguePoints = "",
                    RankedSoloWins = 0,
                    RankedSoloLosses = 0,
                    Trinket = new Db_LccItem()
                    {
                        ItemId = 0,
                        ItemName = "",
                        ImageFull = ""
                    },
                    ItemOne = new Db_LccItem()
                    {
                        ItemId = 0,
                        ItemName = "",
                        ImageFull = ""
                    },
                    ItemTwo = new Db_LccItem()
                    {
                        ItemId = 0,
                        ItemName = "",
                        ImageFull = ""
                    },
                    ItemThree = new Db_LccItem()
                    {
                        ItemId = 0,
                        ItemName = "",
                        ImageFull = ""
                    },
                    ItemFour = new Db_LccItem()
                    {
                        ItemId = 0,
                        ItemName = "",
                        ImageFull = ""
                    },
                    ItemFive = new Db_LccItem()
                    {
                        ItemId = 0,
                        ItemName = "",
                        ImageFull = ""
                    },
                    ItemSix = new Db_LccItem()
                    {
                        ItemId = 0,
                        ItemName = "",
                        ImageFull = ""
                    },
                    SummonerOne = new Db_LccSummonerSpell()
                    {
                        SummonerSpellId = 0,
                        SummonerSpellName = "",
                        ImageFull = ""
                    },
                    SummonerTwo = new Db_LccSummonerSpell()
                    {
                        SummonerSpellId = 0,
                        SummonerSpellName = "",
                        ImageFull = ""
                    },
                    Champion = new Db_LccChampion()
                    {
                        ChampionId = 0,
                        ChampionName = "",
                        ImageFull = ""
                    },
                    ChampionLevel = 0,
                    PrimaryRuneStyle = new Db_LccRune()
                    {
                        RuneId = 0,
                        RuneName = "",
                        RunePathName = "",
                        Icon = "",
                        Key = "",
                        ShortDesc = "",
                        LongDesc = ""
                    },
                    PrimaryRuneSubOne = new Db_LccRune()
                    {
                        RuneId = 0,
                        RuneName = "",
                        RunePathName = "",
                        Icon = "",
                        Key = "",
                        ShortDesc = "",
                        LongDesc = ""
                    },
                    PrimaryRuneSubTwo = new Db_LccRune()
                    {
                        RuneId = 0,
                        RuneName = "",
                        RunePathName = "",
                        Icon = "",
                        Key = "",
                        ShortDesc = "",
                        LongDesc = ""
                    },
                    PrimaryRuneSubThree = new Db_LccRune()
                    {
                        RuneId = 0,
                        RuneName = "",
                        RunePathName = "",
                        Icon = "",
                        Key = "",
                        ShortDesc = "",
                        LongDesc = ""
                    },
                    PrimaryRuneSubFour = new Db_LccRune()
                    {
                        RuneId = 0,
                        RuneName = "",
                        RunePathName = "",
                        Icon = "",
                        Key = "",
                        ShortDesc = "",
                        LongDesc = ""
                    },
                    SecondaryRuneStyle = new Db_LccRune()
                    {
                        RuneId = 0,
                        RuneName = "",
                        RunePathName = "",
                        Icon = "",
                        Key = "",
                        ShortDesc = "",
                        LongDesc = ""
                    },
                    SecondaryRuneSubOne = new Db_LccRune()
                    {
                        RuneId = 0,
                        RuneName = "",
                        RunePathName = "",
                        Icon = "",
                        Key = "",
                        ShortDesc = "",
                        LongDesc = ""
                    },
                    SecondaryRuneSubTwo = new Db_LccRune()
                    {
                        RuneId = 0,
                        RuneName = "",
                        RunePathName = "",
                        Icon = "",
                        Key = "",
                        ShortDesc = "",
                        LongDesc = ""
                    }
                };
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception thrown creating cahcedPlayerStatsFromMatchInfo : " + e.Message);
            }

            return new Db_LccCachedPlayerStats();
        }
        private Db_LccCachedCalculatedMatchupInfo CreateDatabaseModelForCalculatedMatchupInfo(Match match, long usersChampionId)
        {
            IList<Db_LccItem> itemStaticData = _itemStaticDataRepository.GetAllItems().ToList();
            IList<Db_LccRune> runeStaticData = _runeStaticDataReposistory.GetAllRunes().ToList();
            IList<Db_LccChampion> championStaticData = _championStaticDataRepository.GetAllChampions().ToList();
            IList<Db_LccSummonerSpell> summonerSpellStaticData = _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList();

            try
            {
                Db_LccCachedCalculatedMatchupInfo cachedMatchupInformation = new Db_LccCachedCalculatedMatchupInfo()
                {
                    GameId = match.GameId,
                    MatchDate = match.GameCreation,
                    MatchPatch = match.GameVersion,
                    FriendlyTeamWin = false //placeholder value
                };

                cachedMatchupInformation.EnemyTeam = new Db_LccCachedTeamInformation()
                {
                    //placeholder values
                    TotalKills = 0,
                    TotalDeaths = 0,
                    TotalAssists = 0,
                    DragonKills = 0,
                    BaronKills = 0,
                    RiftHeraldKills = 0,
                    InhibitorKills = 0,
                };

                cachedMatchupInformation.FriendlyTeam = new Db_LccCachedTeamInformation()
                {
                    //placeholder values
                    TotalKills = 0,
                    TotalDeaths = 0,
                    TotalAssists = 0,
                    DragonKills = 0,
                    BaronKills = 0,
                    RiftHeraldKills = 0,
                    InhibitorKills = 0,
                };

                //100 or 200
                int usersTeamId = match.Participants.FirstOrDefault(x => x.ChampionId == usersChampionId).TeamId;

                IList<Participant> friendlyTeamParticipants = match.Participants.Where(x => x.TeamId == usersTeamId).ToList();
                IList<Participant> enemyTeamParticipants = match.Participants.Where(x => x.TeamId != usersTeamId).ToList();

                cachedMatchupInformation.EnemyTeam.Players = new List<Db_LccCachedPlayerStats>();
                foreach(Participant enemyParticipant in enemyTeamParticipants)
                {
                    ParticipantIdentity enemyParticipanyIdentity = match.ParticipantIdentities.FirstOrDefault(x => x.ParticipantId == enemyParticipant.ParticipantId);
                    cachedMatchupInformation.EnemyTeam.Players.Add(CreateCachedPlayerStatsFromMatchupInfo(enemyParticipanyIdentity, enemyParticipant));
                }
                
                cachedMatchupInformation.FriendlyTeam.Players = new List<Db_LccCachedPlayerStats>();
                foreach (Participant friendlyParticipant in friendlyTeamParticipants)
                {
                    ParticipantIdentity friendlyParticipanyIdentity = match.ParticipantIdentities.FirstOrDefault(x => x.ParticipantId == friendlyParticipant.ParticipantId);
                    cachedMatchupInformation.EnemyTeam.Players.Add(CreateCachedPlayerStatsFromMatchupInfo(friendlyParticipanyIdentity, friendlyParticipant));
                }

                return cachedMatchupInformation;
            }
            catch(Exception e)
            {
                int i = 0;
                i++;
            }

            return new Db_LccCachedCalculatedMatchupInfo();
        }
    }
}