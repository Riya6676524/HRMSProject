using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class EmployeeProfileModel
    {
        public int Emp_ID { get; set; }
        public  string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string GenderName { get; set; }
        public string DepartmentName { get; set; }
        public string ReportingManagerName { get; set; }
        public long ContactNumber { get; set; }
        public byte[] ProfileImagePath { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public int ZipCode{ get; set; }
        public DateTime? DOB { get; set; }

        public string CountryName { get; set; }
        public string LocationName { get; set; }
        public string currpassword { get; set; }
        public string newpassword { get; set; }
        public string confirmNewPassword { get; set; }
    }
}
