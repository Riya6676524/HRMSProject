using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class LeaveRequestService : GenericService<LeaveRequestModel>, ILeaveRequestService
    {
        protected override string TableName => "LeaveRequest";
        protected override string PrimaryKeyColumn => "RequestID";
    }
}
