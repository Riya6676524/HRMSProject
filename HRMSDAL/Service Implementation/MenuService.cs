using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;

namespace HRMSDAL.Service_Implementation
{
    public class MenuService : GenericService<MenuModel>
        {
            protected override string TableName => "Menu";

            protected override string PrimaryKeyColumn => "MenuID";

        }
    }

