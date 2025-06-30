using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;

namespace HRMSDAL.Service
{
    public interface IEmployeeService : IGenericService<EmployeeModel>
    {
        string GetNextAvailableEmployeeId();
    }
}
