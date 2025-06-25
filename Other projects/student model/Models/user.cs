using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_model.Models
{
    public class user
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int role { get; set; }
        public string rolename {  get; set; }
        public string created_by { get; set; }
        public string modified_by { get; set; }
    }
}