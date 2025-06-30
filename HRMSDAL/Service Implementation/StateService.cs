using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class StateService : GenericService<StateModel>, IStateService
    {
        protected override string TableName => "State";
        protected override string PrimaryKeyColumn => "StateID";
    }
}
