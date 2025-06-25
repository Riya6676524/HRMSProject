using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceProject.Models
{
    public class AttendanceModel
    {
            public DateTime AttendanceDate { get; set; }
            public string AttendanceType { get; set; }
            public int UserID { get; set; }
            public bool IsAdmin { get; set; }
    }
}