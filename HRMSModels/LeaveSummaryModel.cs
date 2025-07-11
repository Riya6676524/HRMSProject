using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class LeaveSummaryModel
    {
        public double TotalAvailable { get; set; }
        public double LeaveTaken { get; set; }
        public double CarryForward { get; set; }
    }

}
