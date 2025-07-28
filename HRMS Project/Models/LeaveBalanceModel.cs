using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS_Project.Models
{
    public class LeaveBalanceModel
    {
        public string LeaveName { get; set; }
        public int LeaveLimit { get; set; }
        public float LeaveTaken { get; set; }
        public float LeaveBalance { get; set; }
    }
}