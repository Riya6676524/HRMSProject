using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Management_System.Models
{
    public class user
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}