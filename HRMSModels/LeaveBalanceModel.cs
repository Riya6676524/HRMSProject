using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class LeaveBalanceModel
    {
        public int Emp_ID { get; set; }

        public float OpeningBalance { get; set; }

        public float Credited { get; set; }

        public float ClosingBalance { get; set; }

        public DateTime ForMonth { get; set; }
    }
}
