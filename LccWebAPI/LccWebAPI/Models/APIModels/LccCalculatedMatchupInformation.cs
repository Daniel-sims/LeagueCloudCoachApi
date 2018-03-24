﻿using System;

namespace LccWebAPI.Models.APIModels
{
    public class LccCalculatedMatchupInformation
    {
        public LccTeamInformation FriendlyTeam { get; set; } = new LccTeamInformation();
        public LccTeamInformation EnemyTeam { get; set; } = new LccTeamInformation();

        public bool FriendlyTeamWon { get; set; }

        public DateTime MatchDate { get; set; }

        public string MatchPatch { get; set; }
    }
}
