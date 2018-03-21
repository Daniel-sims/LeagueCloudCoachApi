using LccWebAPI.Models.APIModels;
using LccWebAPI.Models.DatabaseModels;
using LccWebAPI.Repository.Match;
using Microsoft.AspNetCore.Mvc;
using RiotSharp.Interfaces;
using RiotSharp.MatchEndpoint;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IMatchupInformationRepository _matchupInformationRepository;
        private IRiotApi _riotApi;

        public MatchController(IMatchupInformationRepository matchupInformationRepository, IRiotApi riotApi)
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
            var allMatchesInDatabase = _matchupInformationRepository.GetAllMatchupInformations();
            IList<long> friendlyTeamChampionIds = new List<long>(friendlyTeamChampions) { usersChampionId };
            IList<long> enemyTeamChampionIds = enemyTeamChampions.ToList();


            // This is looks horrible but works....
            // first .Where finds if users champion + lane is on winning team or losing team
            // second .Where checks to see if all of the specified champions are on either team, winning or losing
            // Basically it gets the matches the user specified...
            var matchesContainingUsersChampionAndLane 
                = allMatchesInDatabase
                .Where(x => x.LosingTeam.Any(p => p.ChampionId == usersChampionId && p.Lane.ToLower() == usersLane.ToLower() 
                || x.WinningTeam.Any(u => u.ChampionId == usersChampionId && u.Lane.ToLower() == usersLane.ToLower()))).ToList()
                .Where(q => (enemyTeamChampionIds.All(e => q.LosingTeam.Any(l => l.ChampionId == e)) && friendlyTeamChampionIds.All(f => q.WinningTeam.Any(l => l.ChampionId == f)))
                || (enemyTeamChampionIds.All(e => q.WinningTeam.Any(l => l.ChampionId == e)) && friendlyTeamChampionIds.All(f => q.LosingTeam.Any(l => l.ChampionId == f))));

            List<LccCalculatedMatchupInformation> matchesToReturnToUser = new List<LccCalculatedMatchupInformation>();

            if (matchesContainingUsersChampionAndLane.Any())
            {
                int matchReturnCount = 0;

                foreach(var match in matchesContainingUsersChampionAndLane)
                {
                    if(matchReturnCount != maxMatchLimit)
                    {
                        var riotMatchInformation = await _riotApi.GetMatchAsync(RiotSharp.Misc.Region.euw, match.GameId);
                        matchesToReturnToUser.Add(CreateLccMatchupInformation(riotMatchInformation, usersChampionId));

                        matchReturnCount++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            return new JsonResult(matchesToReturnToUser);
        }

        private LccCalculatedMatchupInformation CreateLccMatchupInformation(Match match, long usersChampionId)
        {
            // General info
            LccCalculatedMatchupInformation matchupInformation = new LccCalculatedMatchupInformation
            {
                MatchDate = match.GameCreation,
                MatchPatch = match.GameVersion
            };

            var usersTeamId = match.Participants.FirstOrDefault(x => x.ChampionId == usersChampionId).TeamId;
            matchupInformation.FriendlyTeamWon =  matchupInformation.FriendlyTeamWon = match.Teams.FirstOrDefault(x => x.TeamId == usersTeamId).Win == "Win";

            // Friendly players

            LccTeamInformation friendlyTeamInformation = new LccTeamInformation
            {
                Kills = match.Participants.Where(x => x.TeamId == usersTeamId).Sum(x => x.Stats.Kills),
                Deaths = match.Participants.Where(x => x.TeamId == usersTeamId).Sum(x => x.Stats.Deaths),
                Assists = match.Participants.Where(x => x.TeamId == usersTeamId).Sum(x => x.Stats.Assists)
            };

            var friendlyParticipants = match.Participants.Where(x => x.TeamId == usersTeamId);
            var friendlyPartidipantIdentits = match.ParticipantIdentities.Where(x => friendlyParticipants.Any(u => u.ParticipantId == x.ParticipantId));

            foreach (var friendlyParticipant in friendlyParticipants)
            {
                var friendlyParticipantIdentity = friendlyPartidipantIdentits.FirstOrDefault(x => x.ParticipantId == friendlyParticipant.ParticipantId);
                friendlyTeamInformation.Players.Add(CreatePlayerInfo(friendlyParticipant, friendlyParticipantIdentity));
            }

            matchupInformation.FriendlyTeam = friendlyTeamInformation;

            // Get enemy players
            LccTeamInformation enemyTeamInformation = new LccTeamInformation
            {
                Kills = match.Participants.Where(x => x.TeamId != usersTeamId).Sum(x => x.Stats.Kills),
                Deaths = match.Participants.Where(x => x.TeamId != usersTeamId).Sum(x => x.Stats.Deaths),
                Assists = match.Participants.Where(x => x.TeamId != usersTeamId).Sum(x => x.Stats.Assists)
            };

            var enemyParticipants = match.Participants.Where(x => x.TeamId != usersTeamId);
            var enemyPartidipantIdentitys = match.ParticipantIdentities.Where(x => enemyParticipants.Any(u => u.ParticipantId == x.ParticipantId));

            foreach (var enemyParticipant in enemyParticipants)
            {
                var enemyParticipantIdentity = enemyPartidipantIdentitys.FirstOrDefault(x => x.ParticipantId == enemyParticipant.ParticipantId);
                enemyTeamInformation.Players.Add(CreatePlayerInfo(enemyParticipant, enemyParticipantIdentity));
            }

            matchupInformation.EnemyTeam = enemyTeamInformation;

            return matchupInformation;
        }

        private LccMatchplayerInformation CreatePlayerInfo(Participant participant, ParticipantIdentity participantIdentity)
        {
            return new LccMatchplayerInformation()
            {
                SummonerName = participantIdentity.Player.SummonerName,
                Division = participant.HighestAchievedSeasonTier.ToString(),

                Kills = participant.Stats.Kills,
                Deaths = participant.Stats.Deaths,
                Assists = participant.Stats.Assists,
                MinionKills = participant.Stats.MinionsKilled,
                Item1Id = participant.Stats.Item0,
                Item2Id = participant.Stats.Item1,
                Item3Id = participant.Stats.Item2,
                Item4Id = participant.Stats.Item3,
                Item5Id = participant.Stats.Item4,
                Item6Id = participant.Stats.Item5,
                TrinketId = participant.Stats.Item6,
                FirstItemsBought = new List<long>(),
                SummonerOne = participant.Spell1Id,
                SummonerTwo = participant.Spell2Id
            };
        }
    }
}