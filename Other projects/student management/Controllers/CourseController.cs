using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace student_management.Controllers
{
    public class CourseController : Controller
    {
        public ActionResult Index()
            {
                // Add code to display courses
                return View();
            }

            public ActionResult Enroll()
            {
                // Add code for enrollment
                return View();
            }
        }
    }

