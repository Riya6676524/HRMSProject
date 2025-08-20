using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;


namespace HRMSProject.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;


        public AttendanceController(IAttendanceService attendanceService, IEmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
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
                return Json(new { success = true, message = "Mode updated successfully." });
            }

            return Json(new { success = false, message = "Mode name cannot be empty." });
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





        public void InitEmpViewBag(int? selectedEmpId = null)
        {
            int loggedInEmpId = Convert.ToInt32(Session["Emp_ID"]);
            int roleId = Convert.ToInt32(Session["RoleID"]);

            var empSelectList = new List<SelectListItem>();

            if (roleId == 1) // Admin -> Show all employees
            {
                var allEmployees = _employeeService.GetAll();
                foreach (var emp in allEmployees)
                {
                    empSelectList.Add(new SelectListItem
                    {
                        Text = $"{emp.FirstName} {emp.Middlename} {emp.LastName}",
                        Value = emp.EMP_ID.ToString(),
                        Selected = (selectedEmpId == emp.EMP_ID || (selectedEmpId == null && emp.EMP_ID == loggedInEmpId))
                    });
                }
            }
            else // Manager/Employee -> Show self + subordinates
            {
                // Add self
                var empIter = _employeeService.GetById(loggedInEmpId);
                if (empIter != null)
                {
                    empSelectList.Add(new SelectListItem
                    {
                        Text = $"{empIter.FirstName} {empIter.Middlename} {empIter.LastName}",
                        Value = empIter.EMP_ID.ToString(),
                        Selected = (selectedEmpId == null || selectedEmpId == loggedInEmpId)
                    });
                }

                // Add subordinates
                var empIDs = _employeeService.GetSubOrdinatesByManager(loggedInEmpId);
                foreach (int id in empIDs)
                {
                    empIter = _employeeService.GetById(id);
                    if (empIter != null)
                    {
                        empSelectList.Add(new SelectListItem
                        {
                            Text = $"{empIter.FirstName} {empIter.Middlename} {empIter.LastName}",
                            Value = empIter.EMP_ID.ToString(),
                            Selected = (selectedEmpId == id)
                        });
                    }
                }
            }

            ViewBag.IDs = empSelectList;
        }


        private JsonResult BuildAttendanceEvents(int empId)
        {
            var today = DateTime.Today;

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

        [HttpGet]
        public JsonResult GetAttendanceEvents(int? EmpId)
        {
            int empIdToUse = EmpId ?? Convert.ToInt32(Session["Emp_ID"]);
            return BuildAttendanceEvents(empIdToUse);
        }

        [HttpGet]
        public ActionResult Manage(int? empId, DateTime? startDate, DateTime? endDate, int? page)
        {
            int loggedInEmpId = Convert.ToInt32(Session["Emp_ID"]);
            int selectedEmpId = empId ?? loggedInEmpId;

            InitEmpViewBag(selectedEmpId);

            DateTime defaultStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime defaultEnd = DateTime.Now;

            DateTime finalStart = startDate ?? defaultStart;
            DateTime finalEnd = endDate ?? defaultEnd;

            ViewBag.SelectedStartDate = finalStart.ToString("yyyy-MM-dd");
            ViewBag.SelectedEndDate = finalEnd.ToString("yyyy-MM-dd");
            ViewBag.SelectedEmpId = selectedEmpId;
            ViewBag.CurrentPage = page ?? 1;

            var attendanceList = _attendanceService.GetAttendanceByStartEndDate(
                selectedEmpId,
                finalStart,
                finalEnd
            );

            return View(attendanceList);
        }


        [HttpPost]
        public ActionResult Manage(int? empId, DateTime? startDate, DateTime? endDate)
        {
            int loggedInEmpId = Convert.ToInt32(Session["Emp_ID"]);
            int selectedEmpId = empId ?? loggedInEmpId;

            InitEmpViewBag(selectedEmpId);

            // Default dates
            DateTime defaultStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime defaultEnd = DateTime.Now;

            // Final dates (use defaults if null)
            DateTime finalStart = startDate ?? defaultStart;
            DateTime finalEnd = endDate ?? defaultEnd;

            // Validation
            if (finalStart > finalEnd)
            {
                ModelState.AddModelError("", "Start Date cannot be greater than End Date.");
                ViewBag.SelectedStartDate = finalStart.ToString("yyyy-MM-dd");
                ViewBag.SelectedEndDate = finalEnd.ToString("yyyy-MM-dd");
                return View(new List<AttendanceModel>());
            }

            // Fetch attendance list
            var attendanceList = _attendanceService.GetAttendanceByStartEndDate(
                selectedEmpId,
                finalStart,
                finalEnd
            );

            // Pass selected dates back to the view
            ViewBag.SelectedStartDate = finalStart.ToString("yyyy-MM-dd");
            ViewBag.SelectedEndDate = finalEnd.ToString("yyyy-MM-dd");

            return View(attendanceList);
        }


        //[HttpGet]
        //public ActionResult Edit(int empId, DateTime attendanceDate)
        //{
        //    var record = _attendanceService.GetAttendance(empId, attendanceDate);

        //    if (record == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(record);
        //}

        //[HttpPost]

        //public ActionResult Edit(AttendanceModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool isUpdated = _attendanceService.UpdateAttendance(model);
        //        if (isUpdated)
        //        {
        //            return RedirectToAction("Manage");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Failed to update attendance.");
        //        }
        //    }
        //    return View(model);
        //}


        [HttpGet]
        public ActionResult EditRequest(int empId, DateTime attendanceDate)
        {
            var record = _attendanceService.GetAttendance(empId, attendanceDate);

            if (record == null)
            {
                return HttpNotFound();
            }

            return View(record);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRequest(AttendanceModel request)
        {
            if (!ModelState.IsValid)
                return View(request);

            // Set initial request data
            request.Status = "Pending";
            request.CreatedOn = DateTime.Now;

            // Save request via service
            _attendanceService.CreateAttendanceRequest(request);

            // Success message
            TempData["SuccessMessage"] = "Attendance edit request submitted successfully.";

            // Redirect back to Attendance grid (or wherever appropriate)
            return RedirectToAction("Manage", "Attendance");
        }



        [HttpGet]
        public ActionResult AttendanceRequest()
        {
            return View();

        }

        [HttpGet]
        public ActionResult WorkStatusModal()
        {
            return PartialView("WorkStatusConfirmationPrompt");
        }
    }
}



