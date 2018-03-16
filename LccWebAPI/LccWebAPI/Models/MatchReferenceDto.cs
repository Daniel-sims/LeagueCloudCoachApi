using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models
{
    public class MatchReferenceDto
    {
        public MatchReferenceDto() { }
        public MatchReferenceDto(MatchReference matchReference)
        {
            MatchReference = matchReference;
        }

        [Key]
        public int Id { get; set; }
        
        public MatchReference MatchReference { get; set; }
    }
}
