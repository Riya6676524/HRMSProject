using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace student_model.Models
{
    public class enrollement
    {
        public int enrollmentid { get; set; }
        public int studentid { get; set; }
        public int courseid { get; set; }
        public DateTime enrollmentdate { get; set; }
    }
}