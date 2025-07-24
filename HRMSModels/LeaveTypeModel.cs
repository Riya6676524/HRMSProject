using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class LeaveTypeModel
    {
        public int LeaveTypeID { get; set; }
        public string LeaveName { get; set; }
        public int LeaveLimits { get; set; }
        public Boolean CarryForward { get; set; }
    }
}
