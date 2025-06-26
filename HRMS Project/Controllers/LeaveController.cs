using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS_Project.Controllers
{
    public class LeaveController : Controller
    {
        // GET: Leave
        public ActionResult Apply()
        {
            return View();
        }
        public ActionResult Manage()
        {
            return View();
        }
    }
}