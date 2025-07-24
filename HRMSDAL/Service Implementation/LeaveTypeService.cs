using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class LeaveTypeService : GenericService<LeaveTypeModel>, ILeaveTypeService
    {
        protected override string TableName => "LeaveType";
        protected override string PrimaryKeyColumn => "LeaveTypeID";
    }
}
