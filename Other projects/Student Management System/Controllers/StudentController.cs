using Student_Management_System.DAL;
using Student_Management_System.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Student_Management_System.Controllers
{
    public class StudentController : Controller
    {
        studentDAL studentDAL = new studentDAL();

        public ActionResult Index()
        {
            List<student> students = studentDAL.GetAllStudents();
            return View(students);
        }

        // Add Create and Edit methods here
    }
}

