using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_model.Models
{
    public class student
    {
        public int studentid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int Role { get; set; }
        public string created_by { get; set; }
        public string modified_by { get; set; }


    }
}