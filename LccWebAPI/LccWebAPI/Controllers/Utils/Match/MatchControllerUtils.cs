using RiotSharp.Interfaces;

namespace LccWebAPI.Controllers.Utils.Match
{
    public class MatchControllerUtils : IMatchControllerUtils
    {
        private readonly IRiotApi _riotApi;

        public MatchControllerUtils(IRiotApi riotApi)
        {
            _riotApi = riotApi;
        }
        
    }
}
   
