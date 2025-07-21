using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HRMSModels
{
    public class LoginModel
    {
        public string Email { get; set; }

        public int Emp_ID { get; set; }
        public string Password { get; set; }

        public int RoleID { get; set; }

        public bool RememberMe { get; set; }
        public string ModeName { get; set; }


    }

}
