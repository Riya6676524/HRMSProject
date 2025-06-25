using FirstProject1.DAL;
using FirstProject1.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Windows.Forms;

public class StudentController : Controller
{
    private studentDAL studDAL = new studentDAL(); 

    //for get
    public ActionResult Create()
    {
        return View();
    }
    //for post

    [HttpPost]
    public ActionResult Create(student stud)
    {
        if (ModelState.IsValid)
        {
            int result = studDAL.AddStudent(stud); // 👈 Call the DAL method

            if (result > 0)
            {
              
                return RedirectToAction("list"); 
            }
            else
            {
              
                ViewBag.Message = "Error while saving data!";
            }
        }
        return View(stud);
    }
  
    public ActionResult list()
    {
        List<student> students = studDAL.GetAllStudents();
        return View(students);
    }
}

