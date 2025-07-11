using HRMSModels;
using HRMSDAL;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

public class AttendanceController : Controller
{
    private AttendanceService attendanceService = new AttendanceService();

    [HttpGet]
    public ActionResult Index()
    {
        AttendanceModel model = new AttendanceModel
        {
            AttendanceDate = DateTime.Today
        };
        ViewBag.ModeList = GetModeList();
        return View(model);
    }

    [HttpPost]
    public ActionResult Index(AttendanceModel model)
    {
        if (ModelState.IsValid)
        {
           
            if (model.SelectedHalf == "Full Day")
            {
                model.FirstHalfStatus = "Present";
                model.SecondHalfStatus = "Present";
            }
            else if (model.SelectedHalf == "First Half")
            {
                model.FirstHalfStatus = "Present";
                model.SecondHalfStatus = "Absent";
            }
            else if (model.SelectedHalf == "Second Half")
            {
                model.FirstHalfStatus = "Absent";
                model.SecondHalfStatus = "Present";
            }

            bool result = attendanceService.MarkAttendance(model);

            if (result)
            {
                TempData["Success"] = "Attendance marked successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to mark attendance.");
            }
        }

        ViewBag.ModeList = GetModeList();
        return View(model);
    }

    [HttpGet]
    public JsonResult GetAttendanceHistory(int empId, int month, int year)
    {
        var data = attendanceService.GetAttendanceHistory(empId, month, year);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    private List<SelectListItem> GetModeList()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Text = "WFO", Value = "2" },
            new SelectListItem { Text = "WFH", Value = "1" }
        };
    }
}
