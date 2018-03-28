using LccWebAPI.Constants;
using LccWebAPI.Controllers.Models.Match;
using LccWebAPI.Controllers.Models.StaticData;
using LccWebAPI.Database.Models.Match;
using LccWebAPI.Database.Models.StaticData;
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
        
        private readonly IItemStaticDataRepository _itemStaticDataRepository;
        private readonly ISummonerSpellStaticDataRepository _summonerSpellStaticDataRepository;
        private readonly IChampionStaticDataRepository _championStaticDataRepository;
        private readonly IRunesStaticDataRepository _runeStaticDataReposistory;

        public MatchController(IRiotApi riotApi,
            IBasicMatchupInformationRepository matchupInformationRepository,
            IItemStaticDataRepository itemStaticDataRepository,
            ISummonerSpellStaticDataRepository summonerSpellStaticDataRepository,
            IChampionStaticDataRepository championStaticDataRepository,
            IRunesStaticDataRepository runeStaticDataReposistory)
        {
            _riotApi = riotApi;

            _matchupInformationRepository = matchupInformationRepository;
            
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
                    
                    Match riotMatchInformation = await _riotApi.Match.GetMatchAsync(Region.euw, match.GameId);
                    matchesToReturnToUser.Add(await CreateLccMatchupInformation(riotMatchInformation, usersChampionId));

                    matchReturnCount++;
                }
            }
            
            return new JsonResult(matchesToReturnToUser);
        }

        private async Task<LccCalculatedMatchupInformation> CreateLccMatchupInformation(Match match, long usersChampionId)
        {
            // General info
            LccCalculatedMatchupInformation matchupInformation = new LccCalculatedMatchupInformation()
            {
                MatchDate = match.GameCreation,
                MatchPatch = match.GameVersion,
                GameId = match.GameId
            };

            int usersTeamId = match.Participants.FirstOrDefault(x => x.ChampionId == usersChampionId).TeamId;

            // FRIENDLY TEAM INFORMATION
            TeamStats friendlyTeam = match.Teams.FirstOrDefault(x => x.TeamId == usersTeamId);
            matchupInformation.FriendlyTeamWin = friendlyTeam?.Win == MatchOutcome.Win;
            
            LccTeamInformation friendlyTeamInformation = new LccTeamInformation
            {
                TotalKills = match.Participants.Where(x => x.TeamId == usersTeamId).Sum(x => x.Stats.Kills),
                TotalDeaths = match.Participants.Where(x => x.TeamId == usersTeamId).Sum(x => x.Stats.Deaths),
                TotalAssists = match.Participants.Where(x => x.TeamId == usersTeamId).Sum(x => x.Stats.Assists),
                DragonKills = friendlyTeam.DragonKills,
                BaronKills = friendlyTeam.BaronKills,
                RiftHeraldKills = friendlyTeam.RiftHeraldKills,
                InhibitorKills = friendlyTeam.InhibitorKills
            };

            List<Participant> friendlyParticipants = match.Participants.Where(x => x.TeamId == usersTeamId).ToList();
            List<ParticipantIdentity> friendlyPartidipantIdentitys = match.ParticipantIdentities.Where(x => friendlyParticipants.Any(u => u.ParticipantId == x.ParticipantId)).ToList();

            foreach (Participant friendlyParticipant in friendlyParticipants)
            {
                ParticipantIdentity friendlyParticipantIdentity = friendlyPartidipantIdentitys.FirstOrDefault(x => x.ParticipantId == friendlyParticipant.ParticipantId);
                List<LeaguePosition> leaguePosition = await _riotApi.League.GetLeaguePositionsAsync(Region.euw, friendlyParticipantIdentity.Player.SummonerId);

                friendlyTeamInformation.Players.Add
                    (
                        CreatePlayerStats
                        ( 
                            friendlyParticipant,
                            friendlyParticipantIdentity,
                            leaguePosition.FirstOrDefault(x => x.QueueType == LeagueQueue.RankedSolo)
                        )
                    );
            }

            matchupInformation.FriendlyTeam = friendlyTeamInformation;

            // ENEMY TEAM INFORMATION
            TeamStats enemyTeam = match.Teams.FirstOrDefault(x => x.TeamId != usersTeamId);

            LccTeamInformation enemyTeamInformation = new LccTeamInformation
            {
                TotalKills = match.Participants.Where(x => x.TeamId != usersTeamId).Sum(x => x.Stats.Kills),
                TotalDeaths = match.Participants.Where(x => x.TeamId != usersTeamId).Sum(x => x.Stats.Deaths),
                TotalAssists = match.Participants.Where(x => x.TeamId != usersTeamId).Sum(x => x.Stats.Assists),
                 DragonKills = enemyTeam.DragonKills,
                BaronKills = enemyTeam.BaronKills,
                RiftHeraldKills = enemyTeam.RiftHeraldKills,
                InhibitorKills = enemyTeam.InhibitorKills
            };

            List<Participant> enemyParticipants = match.Participants.Where(x => x.TeamId != usersTeamId).ToList();
            List<ParticipantIdentity> enemyPartidipantIdentitys = match.ParticipantIdentities.Where(x => enemyParticipants.Any(u => u.ParticipantId == x.ParticipantId)).ToList();
            
            foreach (Participant enemyParticipant in enemyParticipants)
            {
                ParticipantIdentity enemyParticipantIdentity = enemyPartidipantIdentitys.FirstOrDefault(x => x.ParticipantId == enemyParticipant.ParticipantId);
                List<LeaguePosition> leaguePosition = await _riotApi.League.GetLeaguePositionsAsync(Region.euw, enemyParticipantIdentity.Player.SummonerId);

                enemyTeamInformation.Players.Add
                    (
                        CreatePlayerStats
                        (
                            enemyParticipant,
                            enemyParticipantIdentity,
                            leaguePosition.FirstOrDefault(x => x.QueueType == LeagueQueue.RankedSolo)
                        )
                    );
            }

            matchupInformation.EnemyTeam = enemyTeamInformation;
            return matchupInformation;
        }

        private LccPlayerStats CreatePlayerStats(Participant participant, ParticipantIdentity participantIdentity, LeaguePosition leaguePosition)
        {
            try
            {
                IList<Db_LccItem> itemStaticData = _itemStaticDataRepository.GetAllItems().ToList();
                IList<Db_LccRune> runeStaticData = _runeStaticDataReposistory.GetAllRunes().ToList();
                IList<Db_LccChampion> championStaticData = _championStaticDataRepository.GetAllChampions().ToList();
                IList<Db_LccSummonerSpell> summonerSpellStaticData = _summonerSpellStaticDataRepository.GetAllSummonerSpells().ToList();

                return new LccPlayerStats()
                {
                    SummonerName = participantIdentity.Player.SummonerName,
                    Kills = participant.Stats.Kills,
                    Deaths = participant.Stats.Deaths,
                    Assists = participant.Stats.Assists,
                    MinionKills = participant.Stats.TotalMinionsKilled,
                    RankedSoloDivision = leaguePosition?.Rank,
                    RankedSoloTier = leaguePosition?.Tier,
                    RankedSoloLeaguePoints = leaguePosition?.LeaguePoints.ToString(),
                    RankedSoloWins = Convert.ToInt32(leaguePosition?.Wins),
                    RankedSoloLosses = Convert.ToInt32(leaguePosition?.Losses),
                    ItemOne = new LccItemInformation()
                    {
                        ItemId = participant.Stats.Item0,
                        ItemName = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item0)?.ItemName,
                        ImageFull = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item0)?.ImageFull
                    },
                    ItemTwo = new LccItemInformation()
                    {
                        ItemId = participant.Stats.Item1,
                        ItemName = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item1)?.ItemName,
                        ImageFull = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item1)?.ImageFull
                    },
                    ItemThree = new LccItemInformation()
                    {
                        ItemId = participant.Stats.Item2,
                        ItemName = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item2)?.ItemName,
                        ImageFull = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item2)?.ImageFull
                    },
                    ItemFour = new LccItemInformation()
                    {
                        ItemId = participant.Stats.Item3,
                        ItemName = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item3)?.ItemName,
                        ImageFull = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item3)?.ImageFull
                    },
                    ItemFive = new LccItemInformation()
                    {
                        ItemId = participant.Stats.Item4,
                        ItemName = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item4)?.ItemName,
                        ImageFull = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item4)?.ImageFull
                    },
                    ItemSix = new LccItemInformation()
                    {
                        ItemId = participant.Stats.Item5,
                        ItemName = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item5)?.ItemName,
                        ImageFull = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item5)?.ImageFull
                    },
                    Trinket = new LccItemInformation()
                    {
                        ItemId = participant.Stats.Item6,
                        ItemName = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item6)?.ItemName,
                        ImageFull = itemStaticData.FirstOrDefault(x => x.ItemId == participant.Stats.Item6)?.ImageFull
                    },
                    FirstItems = new List<LccItemInformation>()
                    {
                        new LccItemInformation()
                        {
                            ItemId = 0,
                            ItemName = ""
                        },
                        new LccItemInformation()
                        {
                            ItemId = 0,
                            ItemName = ""
                        },
                        new LccItemInformation()
                        {
                            ItemId = 0,
                            ItemName = ""
                        }
                    },
                    SummonerOne = new LccSummonerSpellInformation()
                    {
                        SummonerSpellId = participant.Spell1Id,
                        SummonerSpellName = summonerSpellStaticData.FirstOrDefault(x => x.SummonerSpellId == participant.Spell1Id)?.SummonerSpellName,
                        ImageFull = summonerSpellStaticData.FirstOrDefault(x => x.SummonerSpellId == participant.Spell1Id)?.ImageFull
                    },
                    SummonerTwo = new LccSummonerSpellInformation()
                    {
                        SummonerSpellId = participant.Spell2Id,
                        SummonerSpellName = summonerSpellStaticData.FirstOrDefault(x => x.SummonerSpellId == participant.Spell2Id)?.SummonerSpellName,
                        ImageFull = summonerSpellStaticData.FirstOrDefault(x => x.SummonerSpellId == participant.Spell2Id)?.ImageFull
                    },
                    Champion = new LccChampionInformation()
                    {
                        ChampionId = participant.ChampionId,
                        ChampionName = championStaticData.FirstOrDefault(x => x.ChampionId == participant.ChampionId)?.ChampionName,
                        ImageFull = championStaticData.FirstOrDefault(x => x.ChampionId == participant.ChampionId)?.ImageFull
                    },
                    ChampionLevel = participant.Stats.ChampLevel,
                    PrimaryRuneStyle = new LccRuneInformation()
                    {
                        RuneId = participant.Stats.PerkPrimaryStyle,
                        RuneName = runeStaticData.FirstOrDefault(x => x.RuneId == participant.Stats.Perk0)?.RunePathName
                    },
                    PrimaryRuneSubOne = new LccRuneInformation()
                    {
                        RuneId = participant.Stats.Perk0,
                        RuneName = runeStaticData.FirstOrDefault(x => x.RuneId == participant.Stats.Perk0)?.RuneName
                    },
                    PrimaryRuneSubTwo = new LccRuneInformation()
                    {
                        RuneId = participant.Stats.Perk1,
                        RuneName = runeStaticData.FirstOrDefault(x => x.RuneId == participant.Stats.Perk1)?.RuneName
                    },
                    PrimaryRuneSubThree = new LccRuneInformation()
                    {
                        RuneId = participant.Stats.Perk2,
                        RuneName = runeStaticData.FirstOrDefault(x => x.RuneId == participant.Stats.Perk2)?.RuneName
                    },
                    PrimaryRuneSubFour = new LccRuneInformation()
                    {
                        RuneId = participant.Stats.Perk3,
                        RuneName = runeStaticData.FirstOrDefault(x => x.RuneId == participant.Stats.Perk3)?.RuneName
                    },
                    SecondaryRuneStyle = new LccRuneInformation()
                    {
                        RuneId = participant.Stats.PerkSubStyle,
                        RuneName = runeStaticData.FirstOrDefault(x => x.RuneId == participant.Stats.Perk4)?.RunePathName
                    },
                    SecondaryRuneSubOne = new LccRuneInformation()
                    {
                        RuneId = participant.Stats.Perk4,
                        RuneName = runeStaticData.FirstOrDefault(x => x.RuneId == participant.Stats.Perk4)?.RuneName
                    },
                    SecondaryRuneSubTwo = new LccRuneInformation()
                    {
                        RuneId = participant.Stats.Perk5,
                        RuneName = runeStaticData.FirstOrDefault(x => x.RuneId == participant.Stats.Perk5)?.RuneName
                    }
                };
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception hit creating the player stats!");
            }

            return new LccPlayerStats();
        }
    }
}