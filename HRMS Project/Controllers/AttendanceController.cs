﻿using System;
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
        public ActionResult SetMode(string modeName)
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);

            if (string.IsNullOrWhiteSpace(modeName))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            int modeId = _attendanceService.GetModeIdByName(modeName);

            _attendanceService.UpdateMode(empId, modeId);

            return RedirectToAction("Index", "Dashboard");
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
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            var attendanceList = _attendanceService.GetAttendanceCalendar(empId);
            var holidayList = _attendanceService.GetLocationHoliday(empId);

            var events = new List<object>();

       
            foreach (var att in attendanceList)
            {
                events.Add(new
                {
                    title = att.Status,
                    start = att.Date.ToString("yyyy-MM-dd"),
                    color = (att.Status == "Present" ||
                                  att.Status == "1stHalf: Present" ||
                                   att.Status == "2ndHalf: Present") ? "#28a745" :
                                   (att.Status == "Absent" ? "#dc3545" :
                                   "#ffc107")
                }); 
            }

            foreach (var holiday in holidayList)
            {
                events.Add(new
                {
                    title = holiday.HolidayName,
                    start = holiday.HolidayDate.ToString("yyyy-MM-dd"),
                    display = "background",
                    backgroundColor = "#bfbfbf"
                });
            }

            return Json(events, JsonRequestBehavior.AllowGet);
        }
    }

    }
