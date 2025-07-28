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
        public string EmployeeID { get; }
        public string Name { get; }
        public string Email { get;  }
        public string Phone { get; }


        [DataType(DataType.Date)]
        [DisplayName("Joined On")]
        public DateTime CreatedON { get; }

        [DataType(DataType.Date)]
        [DisplayName("Modified On")]
        public DateTime UpdatedON { get; }

        public EmployeeListModel(EmployeeModel obj)
        {
            this.EmployeeID = obj.EmployeeID;
            this.Name = $"{obj.FirstName} {obj.Middlename} {obj.LastName}";
            this.Email = obj.Email;
            this.CreatedON = obj.CreatedOn;
            this.UpdatedON = obj.ModifiedOn;
        }
    }
}