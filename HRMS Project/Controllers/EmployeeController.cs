using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS_Project.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

 
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Profile()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            var model = _employeeService.GetProfile(empId);
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
        
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(EmployeeModel model)
        {
         

            if (!ModelState.IsValid)
                return View(model);

            int empId = Convert.ToInt32(Session["Emp_ID"]);

            bool result = _employeeService.ChangePassword(empId, model.currpassword, model.newpassword);

            if (result)
            {
                TempData["Message"] = "Password updated successfully!";
                ModelState.Clear();
                return RedirectToAction("ChangePassword");
            }
            else
            {
                ModelState.AddModelError("currpassword", "Invalid current password");
                return View(model);
            }
        }
    }

}





