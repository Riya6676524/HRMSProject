using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMSProject.Models
{
    public class Pager
    {
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }

    }
}