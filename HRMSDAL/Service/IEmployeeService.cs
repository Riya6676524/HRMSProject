using HRMSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HRMSDAL.Service
{
    public interface IEmployeeService : IGenericService<EmployeeModel>
    {
        string GetNextAvailableEmployeeId();
        List<int> GetSubOrdinatesByManager(int empId);
        EmployeeModel GetProfile(int empId);
        bool ChangePassword(int empId, string currentPassword, string newPassword);
    }
}
