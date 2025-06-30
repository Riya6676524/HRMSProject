using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class CityService : GenericService<CityModel>, ICityService
    {
        protected override string TableName => "City";
        protected override string PrimaryKeyColumn => "CityID";
    }
}
