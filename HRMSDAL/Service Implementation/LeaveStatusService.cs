using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class LeaveStatusService : GenericService<LeaveStatusModel>, ILeaveStatusService
    {
        protected override string TableName => "LeaveStatus";
        protected override string PrimaryKeyColumn => "LeaveStatusID";
    }
}
