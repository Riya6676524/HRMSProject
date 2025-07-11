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
        public int LeaveTypeID { get; set; }
        public bool UptoFirstHalf {  get; set; }
        public bool FromSecondHalf { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public DateTime RequestDate { get; set; }
        public string Reason { get; set; }
        public int LeaveStatusID {  get; set; }
        public int ApproverID { get; set; }
        public DateTime ApproverDate { get; set; }
        public string Comment { get; set; }
    }
}
