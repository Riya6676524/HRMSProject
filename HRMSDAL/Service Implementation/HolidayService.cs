using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class HolidayService : GenericService<HolidayModel>, IHolidayService
    {
        protected override string TableName => "Holiday";
        protected override string PrimaryKeyColumn => "HolidayID";
    }
}
