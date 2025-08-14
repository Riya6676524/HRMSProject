using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HRMSModels
{
    public class LeaveRequestModel
    {
        public int RequestID { get; set; }

        [DisplayName("Employee")]
        public int EMP_ID { get; set; }

        [Required]
        [DisplayName("Leave Type")]
        public int LeaveTypeID { get; set; }

        [Required]
        [DisplayName("Upto First Half")]
        public bool FirstHalf {  get; set; }

        [Required]
        [DisplayName("From Second Half")]
        public bool SecondHalf { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateAfter("StartDate")]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [DisplayName("Total Days")]
        public float TotalDays { get; set; }

        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }

        [Required]
        public string Reason { get; set; }
        public int LeaveStatusID {  get; set; }
        public int? ApproverID { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ApproverDate { get; set; }

        public virtual string Comment { get; set; }
    }
}
