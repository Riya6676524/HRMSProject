using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class EmployeeModel
    {
        public int Emp_ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long ContactNumber { get; set; }
        public byte[] ProfileImagePath { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public string StateName { get; set; }
        public string CityName { get;set;}
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Current password is required")]
        public string currpassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public string newpassword { get; set; }

    }

}
