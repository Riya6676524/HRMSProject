using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class GenderService : GenericService<GenderModel>, IGenderService
    {
        protected override string TableName => "Gender";
        protected override string PrimaryKeyColumn => "GenderID";
    }
}
