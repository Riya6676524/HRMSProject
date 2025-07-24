using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class LocationService : GenericService<LocationModel>, ILocationService
    {
        protected override string TableName => "Location";
        protected override string PrimaryKeyColumn => "LocationID";
    }
}
