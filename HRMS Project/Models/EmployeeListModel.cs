using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HRMSModels;

namespace HRMSProject.Models
{
    public class EmployeeListModel
    {
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Created On")]
        public DateTime CreatedON { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Modified On")]
        public DateTime UpdatedON { get; set; }

        public EmployeeListModel(EmployeeModel obj)
        {
            this.EmployeeID = obj.EmployeeID;
            this.Name = $"{obj.FirstName} {obj.Middlename} {obj.LastName}";
            this.Email = obj.Email;
            this.Phone = obj.ContactNumber;
            this.CreatedON = obj.CreatedOn;
            this.UpdatedON = obj.ModifiedOn;
        }
    }
}