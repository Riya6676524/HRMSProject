using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HRMSModels;

namespace HRMSProject.Models
{
    public class EmployeeRegModel : EmployeeModel
    {
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        private HttpPostedFileBase profilePostedFile;

        [DisplayName("Profile Photo")]
        public HttpPostedFileBase ProfilePostedFile
        {
            get
            {
                return profilePostedFile;
            }
            set
            {
                profilePostedFile = value;
                if (value != null && value.ContentLength > 0)
                {
                    byte[] imageData = new byte[value.ContentLength];
                    value.InputStream.Read(imageData, 0, value.ContentLength);
                    this.ProfileImagePath = imageData;
                }
            }
        }

        public EmployeeRegModel(EmployeeModel obj)
        {
            this.EMP_ID = obj.EMP_ID;
            this.ProfileImagePath = obj.ProfileImagePath;
            this.EmployeeID = obj.EmployeeID;
            this.FirstName = obj.FirstName;
            this.Middlename = obj.Middlename;
            this.LastName = obj.LastName;
            this.GenderID = obj.GenderID;
            this.Email = obj.Email;
            this.ContactNumber = obj.ContactNumber;
            this.Address = obj.Address;
            this.CountryID = obj.CountryID;
            this.StateID = obj.StateID;
            this.CityID = obj.CityID;
            this.ZipCode = obj.ZipCode;
            this.RoleID = obj.RoleID;
            this.DepartmentID = obj.DepartmentID;
            this.ReportingManagerID = obj.ReportingManagerID;
            this.LocationID = obj.LocationID;
            this.DOB = obj.DOB;
        }

        public EmployeeRegModel()
        {

        }
    }

}
