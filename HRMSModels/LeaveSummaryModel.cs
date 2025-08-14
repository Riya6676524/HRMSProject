using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class LeaveSummaryModel
    {
        public decimal TotalAvailable { get; set; }
        public decimal LeaveTaken { get; set; }
        public decimal TotalOpening { get; set; }
        public decimal TotalCredited { get; set; }
        public decimal TotalClosing { get; set; }

    }

}

