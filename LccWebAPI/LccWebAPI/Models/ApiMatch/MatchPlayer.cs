using LccWebAPI.Models.ApiStaticData;
using System.Collections.Generic;

namespace LccWebAPI.Models.ApiMatch
{
    public class MatchPlayer
    {
        // General player data
        public long PlayerId { get; set; }

        // Game specific data
        public int TeamId { get; set; }
        public int ParticipantId { get; set; }
        public string HighestAcheivedTierLastSeason { get; set; }

        public long Kills { get; set; }
        public long Deaths { get; set; }
        public long Assists { get; set; }
        public long TotalMinionsKilled { get; set; }

        public ICollection<MatchEvent> Events { get; set; } = new List<MatchEvent>();

        public Champion Champion { get; set; }
        public long ChampionLevel { get; set; }
        
        public Item ItemOne { get; set; }
        public Item ItemTwo { get; set; }
        public Item ItemThree { get; set; }
        public Item ItemFour { get; set; }
        public Item ItemFive { get; set; }
        public Item ItemSix { get; set; }
        public Item Trinket { get; set; }

        public Rune PrimaryRuneStyle { get; set; }
        public Rune PrimaryRuneSubStyleOne { get; set; }
        public Rune PrimaryRuneSubStyleTwo { get; set; }
        public Rune PrimaryRuneSubStyleThree { get; set; }
        public Rune PrimaryRuneSubStyleFour { get; set; }

        public Rune SecondaryRuneStyle { get; set; }
        public Rune SecondaryRuneStyleOne { get; set; }
        public Rune SecondaryRuneStyleTwo { get; set; }

        public SummonerSpell SummonerSpellOne { get; set; }
        public SummonerSpell SummonerSpellTwo { get; set; }
        

        //Gold
        public long GoldEarned { get; set; }
        public long GoldSpent { get; set; }

        //Vision
        public long VisionScore { get; set; }
        public long WardsPlaced { get; set; }
        public long VisionWardsBoughtInGame { get; set; }
        public long SightWardsBoughtInGame { get; set; }

        //Damage dealt/taken
        public long TotalDamageTaken { get; set; }
        public long TotalDamageDealt { get; set; }
        public long TotalDamageDealtToChampions { get; set; }

        public long TrueDamageTaken { get; set; }
        public long TrueDamageDealt { get; set; }
        public long TrueDamageDealtToChampions { get; set; }

        public long MagicDamageTaken { get; set; }
        public long MagicDamageDealt { get; set; }
        public long MagicDamageDealtToChampions { get; set; }

        public long PhysicalDamageTaken { get; set; }
        public long PhysicalDamageDealt { get; set; }
        public long PhysicalDamageDealtToChampions { get; set; }

        public long LargestCriticalStrike { get; set; }

        // Objectives
        public bool FirstTowerAssist { get; set; }
        public bool FirstTowerKill { get; set; }
        public long TurretKills { get; set; }
        public long DamageDealtToTurrets { get; set; }

        public bool FirstInhibitorAssist { get; set; }
        public bool FirstInhibitorKill { get; set; }
        public long InhibitorKills { get; set; }

        public long DamageDealtToObjectives { get; set; }
        public long ObjectivePlayerScore { get; set; }

        //Kills
        public bool FirstBloodAssist { get; set; }
        public bool FirstBloodKill { get; set; }

        public long LargestMultiKill { get; set; }
        public long LargestKillingSpree { get; set; }
        public long PentaKills { get; set; }
        public long QuadraKills { get; set; }
        public long KillingSprees { get; set; }
        public long DoubleKills { get; set; }

        //Farming
        public long NeutralMinionsKilled { get; set; }
        public long NeutralMinionsKilledEnemyJungle { get; set; }
        public long NeutralMinionsKilledTeamJungle { get; set; }

        //Misc
        public long TimeCCingOthers { get; set; }
        public long TotalTimeCrowdControlDealt { get; set; }

        public long TotalHeal { get; set; }
        public long TotalUnitsHealed { get; set; }

        public long TotalScoreRank { get; set; }
    }
}
