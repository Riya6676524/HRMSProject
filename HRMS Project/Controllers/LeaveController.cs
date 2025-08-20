using System;
using System.Collections.Generic;
using System.Configuration;
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
        private static int leaveThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["LeaveThreshold"]);
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveStatusService _leaveStatusService;
        private readonly IEmployeeService _employeeService;
        private readonly IHolidayService _holidayService;
        private readonly ILeaveBalanceService _leaveBalanceService;

        public LeaveController(ILeaveBalanceService leaveBalanceService, IHolidayService holidayService, IEmployeeService employeeService, ILeaveRequestService leaveService, ILeaveTypeService leaveTypeService, ILeaveStatusService leaveStatusService)
        {
            _leaveRequestService = leaveService;
            _leaveTypeService = leaveTypeService;
            _leaveStatusService = leaveStatusService;
            _employeeService = employeeService;
            _holidayService = holidayService;
            _leaveBalanceService = leaveBalanceService;
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

        public ActionResult PastLeavesPartial(int? id, int count = 3)
        {
            int empID = id ?? Convert.ToInt32(Session["Emp_ID"]);
            List<LeaveRequestModel> leaves = _leaveRequestService.GetLeavesByEmp_ID(empID).Take(count).ToList();
            initLeaveListViewBag(leaves);
            return PartialView(leaves);
        }

        [HttpGet]
        public ActionResult LeaveBalanceGridPartial(int? empID = null)
        {
            int empId = empID ?? Convert.ToInt32(Session["Emp_ID"]);
            var leaveBalances = _leaveBalanceService.GetAllMonthByID(empId);
            return View(leaveBalances);
        }

        public ActionResult LeaveBalanceLabelCard(int? empID = null)
        {
            int empId = empID ?? Convert.ToInt32(Session["Emp_ID"]);
            var leaveBalance = _leaveBalanceService.GetByIdandMonth(empId, DateTime.Now);
            return PartialView(leaveBalance);
        }

        public ActionResult UpcomingHolidays(int count = 1)
        {
            var holidays = _holidayService.GetAll();
            var upcomingHolidays = holidays
                    .Where(h => h.HolidayDate >= DateTime.Now)
                    .GroupBy(h => h.HolidayName) // Group by distinct holiday name
                    .Select(g => g.OrderBy(h => h.HolidayDate).First()) // Take earliest date for each name
                    .OrderBy(h => h.HolidayDate) // Order the distinct list
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
                return View("Add", obj);
            }
            obj.TotalDays = CalculateTotalLeaveDays(obj.StartDate, obj.EndDate, obj.SecondHalf, obj.FirstHalf);
            var leaveBalance = _leaveBalanceService.GetByIdandMonth(obj.EMP_ID, DateTime.Now);
            // Check if the employee has enough leave balance
            if (leaveBalance.ClosingBalance - obj.TotalDays < leaveThreshold)
            {
                initFormViewBag();
                ModelState.AddModelError("TotalDays", "You do not have enough leave balance for this request.");
                return View(obj);
            }

            obj.RequestDate = DateTime.Now;


            var emp = _employeeService.GetById(obj.EMP_ID);
            if (emp.ReportingManagerID is null)
            {
                obj.LeaveStatusID = _leaveStatusService.GetAll().Where(x => x.StatusName.ToUpper() == "APPROVED").Select(x => x.LeaveStatusID).FirstOrDefault();
                leaveBalance.ClosingBalance -= obj.TotalDays;
                _leaveBalanceService.UpdateFinalBalanceNSync(leaveBalance.Emp_ID,leaveBalance.ForMonth,leaveBalance.ClosingBalance);
                obj.ApproverID = emp.EMP_ID;
                obj.ApproverDate = DateTime.Now;
            }
            else
            {
                //This Might Return 0 in case there are no corresponding record with "pending"
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
            if (curStatus.LeaveStatusID == 1 || curStatus.LeaveStatusID == 3) //Pending
            {
                if (Action == "APPROVE" || Action == "DENY")
                {
                    var leaveBalance = _leaveBalanceService.GetByIdandMonth(toBeApprovedModel.EMP_ID, toBeApprovedModel.RequestDate);
                    LeaveStatusModel approveStatus = (Action == "APPROVE") ? allStatuses.FirstOrDefault(x => x.LeaveStatusID == 2) : allStatuses.FirstOrDefault(x => x.LeaveStatusID == 3);
                    toBeApprovedModel.LeaveStatusID = approveStatus.LeaveStatusID;
                    toBeApprovedModel.ApproverID = Convert.ToInt32(Session["Emp_ID"]);
                    toBeApprovedModel.Comment = obj.Comment;
                    toBeApprovedModel.ApproverDate = DateTime.Now;
                    if (approveStatus.LeaveStatusID == 2)
                    {
                        if (leaveBalance.ClosingBalance - obj.TotalDays < leaveThreshold)
                        {
                            initFormViewBag();
                            ModelState.AddModelError("TotalDays", "Does not have enough leave balance for this request.");
                            return View(obj);
                        }
                        leaveBalance.ClosingBalance -= toBeApprovedModel.TotalDays;
                        _leaveBalanceService.UpdateFinalBalanceNSync(leaveBalance.Emp_ID,leaveBalance.ForMonth,leaveBalance.ClosingBalance);
                    }
                    _leaveRequestService.Update(toBeApprovedModel);
                }
            }
            else if (curStatus.LeaveStatusID == 2) //Approved
            {
                if (Action == "DENY")
                {
                    var leaveBalance = _leaveBalanceService.GetByIdandMonth(toBeApprovedModel.EMP_ID, toBeApprovedModel.RequestDate);
                    LeaveStatusModel approveStatus = allStatuses.FirstOrDefault(x => x.LeaveStatusID == 3);
                    toBeApprovedModel.LeaveStatusID = approveStatus.LeaveStatusID;
                    toBeApprovedModel.ApproverID = Convert.ToInt32(Session["Emp_ID"]);
                    toBeApprovedModel.Comment = obj.Comment;
                    toBeApprovedModel.ApproverDate = DateTime.Now;
                    leaveBalance.ClosingBalance += toBeApprovedModel.TotalDays;
                    _leaveBalanceService.UpdateFinalBalanceNSync(leaveBalance.Emp_ID,leaveBalance.ForMonth,leaveBalance.ClosingBalance);
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
            LeaveRequestModel toBeApprovedReq = _leaveRequestService.GetById(id);
            LeaveStatusModel curStatus = _leaveStatusService.GetById(toBeApprovedReq.LeaveStatusID);
            List<LeaveStatusModel> allStatuses = _leaveStatusService.GetAll().ToList();
            EmployeeModel toBeApprovedEmp = _employeeService.GetById(toBeApprovedReq.EMP_ID);
            if ((curStatus.LeaveStatusID == 1 && toBeApprovedEmp.EMP_ID == Convert.ToInt16(Session["Emp_ID"])) || // Leave is Pending and Employee Cancels
                ((toBeApprovedEmp.ReportingManagerID is null) && (toBeApprovedEmp.EMP_ID == Convert.ToInt16(Session["Emp_ID"]))) //Employee with no Manager Cancels his own leave
                )
            {
                LeaveStatusModel approveStatus = allStatuses.FirstOrDefault(x => x.LeaveStatusID == 4);//Cancelled
                toBeApprovedReq.LeaveStatusID = approveStatus.LeaveStatusID;
                if (toBeApprovedEmp.ReportingManagerID is null)
                {
                    var leaveBalance = _leaveBalanceService.GetByIdandMonth(toBeApprovedReq.EMP_ID, toBeApprovedReq.RequestDate);
                    leaveBalance.ClosingBalance += toBeApprovedReq.TotalDays;
                    _leaveBalanceService.UpdateFinalBalanceNSync(leaveBalance.Emp_ID,leaveBalance.ForMonth,leaveBalance.ClosingBalance);
                }
                _leaveRequestService.Update(toBeApprovedReq);
            }
            return RedirectToAction("Leaves", "Leave");
        }

        [HttpGet]
        public JsonResult CalculateLeaveDays(DateTime fromDate, DateTime toDate, bool fromSecondHalf = false, bool uptoFirstHalf = false)
        {
            return Json(new { leaveDays = CalculateTotalLeaveDays(fromDate, toDate, fromSecondHalf, uptoFirstHalf) }, JsonRequestBehavior.AllowGet);
        }

        private float CalculateTotalLeaveDays(DateTime fromDate, DateTime toDate, bool fromSecondHalf = false, bool uptoFirstHalf = false)
        {
            DateUtility dateUtility = new DateUtility();
            float result = dateUtility
                .setFrom(fromDate)
                .setTo(toDate)
                .setFromSecondHalf(fromSecondHalf)
                .setUptoFirstHalf(uptoFirstHalf)
                .CalculateTotalLeaveDays();
            return result;
        }
    }
}