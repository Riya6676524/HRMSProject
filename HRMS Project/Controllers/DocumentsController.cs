using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS_Project.Controllers
{
    public class DocumentsController : Controller
    {
        // GET: Documents
        public ActionResult HRPolicies()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}