using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class DepartmentService : GenericService<DepartmentModel>, IDepartmentService
    {
        protected override string TableName => "Department";
        protected override string PrimaryKeyColumn => "DepartmentID";
    }
}
