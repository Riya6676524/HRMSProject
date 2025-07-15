using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;

namespace HRMSDAL.Service
{
    public interface ILeaveRequestService : IGenericService<LeaveRequestModel>
    {
        List<LeaveRequestModel> GetLeavesByEmp_ID(int empId);
        List<LeaveRequestModel> GetLeavesByManager(int managerID);

    }
}
