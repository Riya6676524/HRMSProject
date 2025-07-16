using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class LeaveRequestModel
    {
        public int RequestID { get; set; }
        public int EMP_ID { get; set; }

        [Required]
        public int LeaveTypeID { get; set; }

        [Required]
        public bool UptoFirstHalf {  get; set; }

        [Required]
        public bool FromSecondHalf { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
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
