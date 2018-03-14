using RiotSharp.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models
{
    public class SummonerDto
    {
        public SummonerDto(Summoner summoner)
        {
            Summoner = summoner;
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Summoner Summoner { get; set; }
    }
}
