using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class MenuModel
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string IconeClass { get; set; }
        public int DisplayOrder { get; set; }

        public int? ParentMenuID { get; set; }

<<<<<<< HEAD
       
=======
       // [Browsable(false)]
       // public List<MenuModel> SubMenus { get; set; } = new List<MenuModel>();
>>>>>>> c905f3f3de17fc9d98420655942a625019de01ba
    }
        
}
