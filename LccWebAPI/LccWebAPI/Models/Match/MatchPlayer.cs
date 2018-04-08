using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LccWebAPI.Models.Match
{
    public class MatchPlayer
    {
        // Primary Key
        [JsonIgnore]
        public int Id { get; set; }
        
        // General player data
        public long PlayerId { get; set; }
        public string HighestAcheivedTierLastSeason { get; set; }

        // Game specific data
        public int TeamId { get; set; }
        public int ParticipantId { get; set; }

        public long Kills { get; set; }
        public long Deaths { get; set; }
        public long Assists { get; set; }
        public long TotalMinionsKilled { get; set; }

        public int ChampionId { get; set; }
        public long ChampionLevel { get; set; }
       
        public long TrinketId { get; set; }
        public long Item1Id { get; set; }
        public long Item2Id { get; set; }
        public long Item3Id { get; set; }
        public long Item4Id { get; set; }
        public long Item5Id { get; set; }
        public long Item6Id { get; set; }

        public long PrimaryRuneStyleId { get; set; }
        public long PrimaryRuneSubStyleOneId { get; set; }
        public long PrimaryRuneSubStyleTwoId { get; set; }
        public long PrimaryRuneSubStyleThreeId { get; set; }
        public long PrimaryRuneSubStyleFourId { get; set; }

        public long SecondaryRuneStyleId { get; set; }
        public long SecondaryRuneSubStyleOneId { get; set; }
        public long SecondaryRuneSubStyleTwoId { get; set; }
        
        public long SummonerSpellOneId { get; set; }
        public long SummonerSpellTwoId { get; set; }
        
        public virtual ICollection<MatchEvent> Events { get; set; }

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

        //Nusc
        public long TimeCCingOthers { get; set; }
        public long TotalTimeCrowdControlDealt { get; set; }

        public long TotalHeal { get; set; }
        public long TotalUnitsHealed { get; set; }
        
        public long TotalScoreRank { get; set; }

        // Foreign Key
        [JsonIgnore]
        public long MatchTeamId { get; set; }
        [JsonIgnore]
        public virtual MatchTeam MatchTeam { get; set; }
    }
}
