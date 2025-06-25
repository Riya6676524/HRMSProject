using Student_Management_System.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Student_Management_System.Controllers
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
