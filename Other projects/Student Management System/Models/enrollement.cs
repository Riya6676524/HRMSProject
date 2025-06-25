using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Management_System.Models
{
    public class enrollement
    {
        public int enrollmentid { get; set; }
        public int studentid { get; set; }
        public int courseid { get; set; }
        public DateTime enrollmentdate { get; set; }
    }
}