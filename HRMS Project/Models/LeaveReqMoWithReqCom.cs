using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HRMSModels;

namespace HRMS_Project.Models
{
    public class LeaveReqMoWithReqCom : LeaveRequestModel
    {
        [Required]
        public override string Comment { get => base.Comment; set => base.Comment = value; }
    }
}