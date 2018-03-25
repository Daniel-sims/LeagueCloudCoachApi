using LccWebAPI.Constants;
using LccWebAPI.Controllers.Models.Match;
using LccWebAPI.Controllers.Models.StaticData;
using LccWebAPI.Database.Models.Match;
using LccWebAPI.Repository.Interfaces.Match;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Interfaces;
using RiotSharp.Misc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IBasicMatchupInformationRepository _matchupInformationRepository;
        private IRiotApi _riotApi;

        public MatchController(IBasicMatchupInformationRepository matchupInformationRepository, IRiotApi riotApi)
        {
            _matchupInformationRepository = matchupInformationRepository;
            _riotApi = riotApi;
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
                    matchesToReturnToUser.Add(CreateLccMatchupInformation(riotMatchInformation, usersChampionId));

                    matchReturnCount++;
                }
            }
            
            return new JsonResult(matchesToReturnToUser);
        }

        private LccCalculatedMatchupInformation CreateLccMatchupInformation(Match match, long usersChampionId)
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
                friendlyTeamInformation.Players.Add
                    (
                        CreatePlayerStats
                        ( 
                            friendlyParticipant, 
                            friendlyPartidipantIdentitys.FirstOrDefault(x => x.ParticipantId == friendlyParticipant.ParticipantId)
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

            var enemyParticipants = match.Participants.Where(x => x.TeamId != usersTeamId);
            var enemyPartidipantIdentitys = match.ParticipantIdentities.Where(x => enemyParticipants.Any(u => u.ParticipantId == x.ParticipantId));

            foreach (Participant enemyParticipant in enemyParticipants)
            {
                ParticipantIdentity enemyParticipantIdentity = enemyPartidipantIdentitys.FirstOrDefault(x => x.ParticipantId == enemyParticipant.ParticipantId);
                enemyTeamInformation.Players.Add
                    (
                        CreatePlayerStats
                        (
                            enemyParticipant,
                            enemyPartidipantIdentitys.FirstOrDefault(x => x.ParticipantId == enemyParticipant.ParticipantId)
                        )
                    );
            }

            matchupInformation.EnemyTeam = enemyTeamInformation;
            return matchupInformation;
        }

        private LccPlayerStats CreatePlayerStats(Participant participant, ParticipantIdentity participantIdentity)
        {
            return new LccPlayerStats()
            {
                SummonerName = participantIdentity.Player.SummonerName,
                Kills = 0,
                Deaths = 0,
                Assists = 0,
                MinionKills = 0,
                Trinket = new LccItemInformation()
                {
                    ItemId = 0,
                    ItemName =""
                },
                ItemOne = new LccItemInformation()
                {
                    ItemId = 0,
                    ItemName = ""
                },
                ItemTwo = new LccItemInformation()
                {
                    ItemId = 0,
                    ItemName = ""
                },
                ItemThree = new LccItemInformation()
                {
                    ItemId = 0,
                    ItemName = ""
                },
                ItemFour = new LccItemInformation()
                {
                    ItemId = 0,
                    ItemName = ""
                },
                ItemFive = new LccItemInformation()
                {
                    ItemId = 0,
                    ItemName = ""
                },
                ItemSix = new LccItemInformation()
                {
                    ItemId = 0,
                    ItemName = ""
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
                    SummonerSpellId = 0,
                    SummonerSpellName = ""
                },
                SummonerTwo = new LccSummonerSpellInformation()
                {
                    SummonerSpellId = 0,
                    SummonerSpellName = ""
                },
                Champion = new LccChampionInformation()
                {
                    ChampionId = 0,
                    ChampionName = ""
                },
                ChampionLevel = 0,
                PrimaryRuneStyle = new LccRuneInformation()
                {
                    RuneId = 0,
                    RuneName = ""
                },
                PrimaryRuneSubOne = new LccRuneInformation()
                {
                    RuneId = 0,
                    RuneName = ""
                },
                PrimaryRuneSubTwo = new LccRuneInformation()
                {
                    RuneId = 0,
                    RuneName = ""
                },
                PrimaryRuneSubThree = new LccRuneInformation()
                {
                    RuneId = 0,
                    RuneName = ""
                },
                SecondaryRuneStyle = new LccRuneInformation()
                {
                    RuneId = 0,
                    RuneName = ""
                },
                SecondaryRuneSubOne = new LccRuneInformation()
                {
                    RuneId = 0,
                    RuneName = ""
                },
                SecondaryRuneSubTwo = new LccRuneInformation()
                {
                    RuneId = 0,
                    RuneName = ""
                }
            };
        }
    }
}