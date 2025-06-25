using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;

namespace HRMSDAL.Service_Implementation
{
    public class RoleMenuService : GenericService<RoleMenuModel>
    {
        protected override string TableName => "RoleMenu";

        protected override string PrimaryKeyColumn => "RoleMenuID";

    }
   
}
