using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class HolidayModel
    {
        public int HolidayID { get; set; }
        public string HolidayName { get; set; }
        public DateTime HolidayDate { get; set; }
        public int LocationID { get; set; }
        public bool Repeat { get; set; }

    }
}
