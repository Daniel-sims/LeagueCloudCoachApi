using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Models.APIModels
{
    public class LccPlayerMasterys
    {
        public long PrimaryMasteryStyleId { get; set; }

        public long SubPrimaryMasteryOneId { get; set; }

        public long SubPrimaryMasteryTwoId { get; set; }

        public long SubPrimaryMasteryThreeId { get; set; }

        public long SubPrimaryMasteryFourId { get; set; }

        public long SecondaryMasteryStyleId { get; set; }

        public long SubSecondaryMasteryOneId { get; set; }

        public long SubSecondaryMasteryTwoId { get; set; }
    }
}
