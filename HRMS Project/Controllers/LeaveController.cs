using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS_Project.Models;
using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;
using HRMSProject.Models;
using HRMSUtility;

namespace HRMS.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveStatusService _leaveStatusService;
        private readonly IEmployeeService _employeeService;
        private readonly IHolidayService _holidayService;

        public LeaveController(IHolidayService holidayService,IEmployeeService employeeService, ILeaveRequestService leaveService, ILeaveTypeService leaveTypeService, ILeaveStatusService leaveStatusService)
        {
            _leaveRequestService = leaveService;
            _leaveTypeService = leaveTypeService;
            _leaveStatusService = leaveStatusService;
            _employeeService = employeeService;
            _holidayService = holidayService;
        }

        public void initFormViewBag()
        {
            int empID = Convert.ToInt32(Session["Emp_ID"]);
            var LeaveTypes = _leaveTypeService.GetAll();
            var empIDs = _employeeService.GetSubOrdinatesByManager(empID);
            var empSelectList = new List<SelectListItem>();
            var empIter = _employeeService.GetById(empID);
            empSelectList.Add(new SelectListItem() { Text = $"{empIter.FirstName} {empIter.Middlename} {empIter.LastName}", Value = empIter.EMP_ID.ToString(), Selected = true });
            foreach (int id in empIDs)
            {
                empIter = _employeeService.GetById(id);
                empSelectList.Add(new SelectListItem() { Text = $"{empIter.FirstName} {empIter.Middlename} {empIter.LastName}", Value = empIter.EMP_ID.ToString() });
            }
            ViewBag.IDs = empSelectList;
            ViewBag.LeaveTypes = new SelectList(LeaveTypes, "LeaveTypeID", "LeaveName");
        }

        public void initLeaveListViewBag(List<LeaveRequestModel> leaveRequests)
        {
            var Employees = leaveRequests
                        .Select(lr => lr.EMP_ID)
                        .Union(leaveRequests.Select(lr => lr.ApproverID ?? lr.EMP_ID))
                        .ToDictionary(
                            empId => empId,
                            empId => _employeeService.GetById(empId)
                        );
            ViewBag.LeaveListData = new LeaveListViewData()
            {
                LeaveStatuses = _leaveStatusService.GetAll().ToDictionary(x => x.LeaveStatusID, x => x.StatusName),
                LeaveTypes = _leaveTypeService.GetAll().ToDictionary(x => x.LeaveTypeID, x => x.LeaveName),
                EmployeeNames = Employees.ToDictionary(
                    kvp => kvp.Key,
                    kvp => $"{kvp.Value.FirstName} {kvp.Value.Middlename} {kvp.Value.LastName}"
                ),
                EmployeeID = Employees.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.EmployeeID)
            };
        }

        public ActionResult PastLeavesPartial(int? id,int count = 3)
        {
            int empID = Convert.ToInt32(Session["Emp_ID"]);
            if (!(id is null))
            {
                empID = id ?? Convert.ToInt32(Session["Emp_ID"]);
            }
            List<LeaveRequestModel> leaves = _leaveRequestService.GetLeavesByEmp_ID(empID).Take(count).ToList();
            initLeaveListViewBag(leaves);
            return PartialView(leaves);
        }

        [HttpGet]
        public ActionResult LeaveBalancePartial()
        {
            var balance = new List<LeaveBalanceModel>();
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            var leaveTypes = _leaveTypeService.GetAll();
            var leaveHistory = _leaveRequestService.GetLeavesByEmp_ID(empId);
            foreach (var leaveType in leaveTypes)
            {
                var leaveBalance = new LeaveBalanceModel()
                {
                    LeaveName = leaveType.LeaveName,
                    LeaveLimit = leaveType.LeaveLimits,
                    LeaveTaken = leaveHistory.Where(x => leaveType.LeaveTypeID == x.LeaveTypeID).Sum(x => x.TotalDays),
                };
                leaveBalance.LeaveBalance = leaveBalance.LeaveLimit - leaveBalance.LeaveTaken;
                balance.Add(leaveBalance);
            }
            return PartialView(balance);
        }

        [HttpGet]
        public ActionResult UpcomingHolidays(int count = 1)
        {
            var holidays = _holidayService.GetAll();
            var upcomingHolidays = holidays.Where(h => h.HolidayDate >= DateTime.Now)
                .OrderBy(h => h.HolidayDate)
                .Take(count)
                .ToList();
            return PartialView("UpcomingHolidayPartial", upcomingHolidays);
        }

        public ActionResult Leaves()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            List<int> allIDS = _employeeService.GetSubOrdinatesByManager(empId);
            var personLeaves = _leaveRequestService.GetLeavesByEmp_ID(empId);
            var allLeaves = new List<LeaveRequestModel>();
            allLeaves.AddRange(personLeaves);
            foreach (int id in allIDS)
            {
                allLeaves.AddRange(_leaveRequestService.GetLeavesByEmp_ID(id));
            }
            initLeaveListViewBag(allLeaves);
            return View(allLeaves);
        }

        public ActionResult Add()
        {

            initFormViewBag();
            return View();
        }

        [HttpPost]
        public ActionResult Add(LeaveRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                initFormViewBag();
                return View(obj);
            }
            else if (obj.StartDate > obj.EndDate)
            {
                ModelState.AddModelError("StartDate", "Start Date must be before End Date.");
            }
            obj.EMP_ID = Convert.ToInt32(Session["Emp_ID"]);
            obj.RequestDate = DateTime.Now;

            //This Might Return 0 in case there are no corresponding record with "pending"
            var emp = _employeeService.GetById(obj.EMP_ID);
            if (emp.ReportingManagerID is null)
            {
                obj.LeaveStatusID = _leaveStatusService.GetAll().Where(x => x.StatusName.ToUpper() == "APPROVED").Select(x => x.LeaveStatusID).FirstOrDefault();
                obj.ApproverID = emp.EMP_ID;
                obj.ApproverDate = DateTime.Now;
            }
            else {
                obj.LeaveStatusID = _leaveStatusService.GetAll().Where(x => x.StatusName.ToUpper() == "PENDING").Select(x => x.LeaveStatusID).FirstOrDefault();
            }
            _leaveRequestService.Insert(obj);
            return RedirectToAction("Leaves", "Leave");
        }

        public ActionResult Approve(int id)
        {
            initFormViewBag();
            LeaveRequestModel toBeApprovedModel = _leaveRequestService.GetById(id);
            return View(toBeApprovedModel);
        }

        [HttpPost]
        public ActionResult Approve(int id, string Action, LeaveRequestModel obj)
        {
            LeaveRequestModel toBeApprovedModel = _leaveRequestService.GetById(id);
            LeaveStatusModel curStatus = _leaveStatusService.GetById(toBeApprovedModel.LeaveStatusID);
            List<LeaveStatusModel> allStatuses = _leaveStatusService.GetAll().ToList();
            if (curStatus.StatusName.ToUpper() == "PENDING")
            {
                if (Action == "APPROVE" || Action == "DENY")
                {
                    LeaveStatusModel approveStatus = (Action == "APPROVED") ? allStatuses.FirstOrDefault(x => x.StatusName.ToUpper() == "APPROVED") : allStatuses.FirstOrDefault(x => x.StatusName.ToUpper() == "REJECTED");
                    toBeApprovedModel.LeaveStatusID = approveStatus.LeaveStatusID;
                    toBeApprovedModel.ApproverID = Convert.ToInt32(Session["Emp_ID"]);
                    toBeApprovedModel.Comment = obj.Comment;
                    toBeApprovedModel.ApproverDate = DateTime.Now;
                    _leaveRequestService.Update(toBeApprovedModel);
                }
            }
            return RedirectToAction("Leaves", "Leave");
        }

        public ActionResult LeaveGridPartial()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            List<int> allIDS = _employeeService.GetSubOrdinatesByManager(empId);
            var personLeaves = _leaveRequestService.GetLeavesByEmp_ID(empId);
            var allLeaves = new List<LeaveRequestModel>();
            foreach (int id in allIDS)
            {
                allLeaves.AddRange(_leaveRequestService.GetLeavesByEmp_ID(id));
            }
            initLeaveListViewBag(allLeaves);
            return PartialView("LeaveGridPartial", allLeaves);
        }

        public ActionResult Cancel(int id)
        {
            LeaveRequestModel toBeApprovedModel = _leaveRequestService.GetById(id);
            LeaveStatusModel curStatus = _leaveStatusService.GetById(toBeApprovedModel.LeaveStatusID);
            List<LeaveStatusModel> allStatuses = _leaveStatusService.GetAll().ToList();
            if (curStatus.StatusName.ToUpper() == "PENDING")
            {
                LeaveStatusModel approveStatus = allStatuses.FirstOrDefault(x => x.StatusName.ToUpper() == "CANCELLED");
                    toBeApprovedModel.LeaveStatusID = approveStatus.LeaveStatusID;
                    _leaveRequestService.Update(toBeApprovedModel);
            }
            return RedirectToAction("Leaves", "Leave");
        }

        [HttpGet]
        public JsonResult CalculateLeaveDays(DateTime fromDate, DateTime toDate, bool fromSecondHalf = false, bool uptoFirstHalf = false)
        {

            DateUtility dateUtility = new DateUtility();
            double result = dateUtility
                .setFrom(fromDate)
                .setTo(toDate)
                .setFromSecondHalf(fromSecondHalf)
                .setUptoFirstHalf(uptoFirstHalf)
                .CalculateTotalLeaveDays();
            return Json(new { leaveDays = result }, JsonRequestBehavior.AllowGet);
        }
    }
}