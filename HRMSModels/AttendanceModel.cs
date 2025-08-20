using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace HRMSModels
{
    public class AttendanceModel
    {
        public int AttendanceID { get; set; }
        public int Emp_ID { get; set; }
        public DateTime AttendanceDate { get; set; }

        public string FirstHalfStatus { get; set; }
        public string SecondHalfStatus { get; set; }

        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }

        public String AttendanceMode { get; set; }
        public int? ModeID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Date { get; set; }
        public string FullStatus { get; set; }
        public string SelectedHalf { get; set; }

        public string HolidayName { get; set; }
        public DateTime HolidayDate { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public double WorkingHours { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsWeekend { get; set; }
        [StringLength(500)]
        public string Reason { get; set; }
        [Display(Name = "Status")]
        public string Status{ get; set; } // Pending / Approved / Rejected

    }
}
