using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{   
    public class RoleService : GenericService<RoleModel>, IRoleService
    {
        protected override string TableName => "Role";
        protected override string PrimaryKeyColumn => "RoleID";
    }
}
