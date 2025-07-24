using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class CountryService : GenericService<CountryModel>, ICountryService
    {
        protected override string TableName => "Country";
        protected override string PrimaryKeyColumn => "CountryID";
    }
}
