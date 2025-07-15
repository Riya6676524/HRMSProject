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

        [DisplayName("PHOTO")]
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
                    this.ProfilePath = imageData;
                }
            }
        }
    }

}
