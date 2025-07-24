using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS_Project.Models
{
    public class LeaveListViewData
    {
        public Dictionary<int, string> LeaveTypes { get; set; }
        public Dictionary<int, string> LeaveStatuses { get; set; }
        public Dictionary<int, string> EmployeeNames { get; set; }
        public Dictionary<int, string> EmployeeID { get; set; }

    }
}