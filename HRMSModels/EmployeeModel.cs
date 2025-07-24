using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HRMS_Project.Models;

namespace HRMSModels
{
    public class EmployeeModel
    {
        public int EMP_ID { get; set; }

        public byte[] ProfileImagePath { get; set; }

        [Required]
        public string EmployeeID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string Middlename { get; set; }

        [Required]
        [DisplayName("LastName")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Gender")]
        public int GenderID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Contact Number")]
        public string ContactNumber {  get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DisplayName("Country")]
        public int CountryID { get; set; }

        [Required]
        [DisplayName("State")]
        public int StateID { get; set; }

        [Required]
        [DisplayName("City")]
        public int CityID { get; set; }

        [Required]
        public int ZipCode { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [MinimumAge(18, ErrorMessage = "You must be at least 18 years old to register.")]
        [DisplayName("Date Of Birth")]
        public DateTime DOB { get; set; }

        [Required]
        [DisplayName("Department")]
        public int DepartmentID { get; set; }

        [Required]
        [DisplayName("Role")]
        public int RoleID  { get; set; }


        [DisplayName("Manager")]
        public int? ReportingManagerID { get; set; }

        public int? CreatedByID { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? ModifiedByID { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
      
      [Required(ErrorMessage = "Current password is required")]
        public string currpassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public string newpassword { get; set; }
        public string RoleName { get; set; }
        public string StateName { get; set; }
        public string CityName { get;set;}
        public string CountryName { get; set; }  
    }

}
