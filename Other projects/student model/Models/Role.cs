using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_model.Models
{
    public class Role
    {
        public int roleid { get; set; }
        public string rolename { get; set; }
        public bool isActive { get; set; }
    }
}