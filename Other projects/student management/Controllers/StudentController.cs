using Data_Access_Layer.DAL;
using student_model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace student_management.Controllers
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

