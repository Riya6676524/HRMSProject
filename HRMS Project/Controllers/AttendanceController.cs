using System;
using System.Collections.Generic;
using System.Drawing;
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
        [ValidateAntiForgeryToken]
        public ActionResult SetMode(string modeName)
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);

            if (!string.IsNullOrWhiteSpace(modeName))
            {
                int modeId = _attendanceService.GetModeIdByName(modeName);
                _attendanceService.UpdateMode(empId, modeId);
            }

            return Json(new { }); 
        }




        [HttpGet]
        public JsonResult GetTodayMode()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            string mode = _attendanceService.GetTodayModeName(empId);
            return Json(new { modeName = mode }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult MarkLogout()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            _attendanceService.MarkLogoutTime(empId);
            return RedirectToAction("Index", "Dashboard");
        }


        [HttpGet]
        public JsonResult GetAttendanceEvents()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);

            var today = DateTime.Today;

            // Only fetch up to yesterday
            var attendanceList = _attendanceService
                .GetAttendanceCalendar(empId)
                .Where(a => a.Date < today) // exclude today
                .ToList();

            var holidayList = _attendanceService.GetLocationHoliday(empId);

            var events = new List<object>();

            // Attendance events
            foreach (var att in attendanceList)
            {
                events.Add(new
                {
                    title = att.Status,
                    start = att.Date.ToString("yyyy-MM-dd"),
                    color = (att.Status == "Present") ? "#28a745" :
                             (att.Status == "Absent" ? "#dc3545" : "#ffc107"),
                    extendedProps = new
                    {
                    fullStatus = att.FullStatus 
                    }
                });
            }

            // Holiday events
            foreach (var holiday in holidayList)
            {
                events.Add(new
                {
                    title = holiday.HolidayName,
                    start = holiday.HolidayDate.ToString("yyyy-MM-dd"),
                    display = "background",
                    backgroundColor = "#bfbfbf",
                     interactive = true
                });
            }

            return Json(events, JsonRequestBehavior.AllowGet);
        }

    }

}
