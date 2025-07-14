using System;
using System.Linq;
using System.Web.Mvc;
using HRMSDAL.Service;


namespace HRMSProject.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetMode(string modeName)
        {   
                int empId = Convert.ToInt32(Session["Emp_ID"]);

                if (string.IsNullOrWhiteSpace(modeName))
                {
                    TempData["Error"] = "Invalid mode selection.";
                    return RedirectToAction("Index", "Dashboard");
                }


           
                Session["SelectedMode"] = modeName;

                return RedirectToAction("Index", "Dashboard");
            }

        [HttpGet]
        public JsonResult GetSelectedMode()
        {
            var mode = Session["SelectedMode"]?.ToString();
            return Json(new { modeName = mode }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAttendanceEvents()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            var data = _attendanceService.GetAttendanceCalendar(empId, DateTime.Now.Year, DateTime.Now.Month);

            var json = data.Select(x => new
            {
                title = x.Status,
                start = x.Date.ToString("yyyy-MM-dd"),
                color = x.Status == "Present" ? "#28a745" :
                        x.Status == "Absent" ? "#dc3545" :
                        "#6c757d" 
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }

}
