using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public class MenuService : GenericService<MenuModel>,IMenuService
        {
            protected override string TableName => "Menu";

            protected override string PrimaryKeyColumn => "MenuID";

        }
    }

