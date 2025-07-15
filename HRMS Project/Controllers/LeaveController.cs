using System;
using System.Collections.Generic;
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
    [DummyDataFilter]
    public class LeaveController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveStatusService _leaveStatusService;
        private readonly IEmployeeService _employeeService;

        public LeaveController(IEmployeeService employeeService, ILeaveRequestService leaveService, ILeaveTypeService leaveTypeService, ILeaveStatusService leaveStatusService)
        {
            _leaveRequestService = leaveService;
            _leaveTypeService = leaveTypeService;
            _leaveStatusService = leaveStatusService;
            _employeeService = employeeService;
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

        public ActionResult LeaveRequests(int pg = 1)
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            var allLeaveRequests = _leaveRequestService.GetLeavesByManager(empId);
            var leaveRequests = allLeaveRequests.Skip((pg - 1) * 10).Take(10).ToList();
            ViewBag.pager = new Pager() { PageCount = allLeaveRequests.Count / 10, PageSize = 10, CurrentPage = pg };
            initLeaveListViewBag(leaveRequests);
            return View(leaveRequests);
        }

        public ActionResult Leaves(int pg = 1)
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            var allLeaves = _leaveRequestService.GetLeavesByEmp_ID(empId);
            var leaves = allLeaves.Skip((pg - 1) * 10).Take(10).ToList();
            ViewBag.pager = new Pager() { PageCount = allLeaves.Count / 10, PageSize = 10, CurrentPage = pg };
            initLeaveListViewBag(leaves);
            return View(leaves);
        }

        public ActionResult Add()
        {
            var LeaveTypes = _leaveTypeService.GetAll();
            ViewBag.LeaveTypes = new SelectList(LeaveTypes, "LeaveTypeID", "LeaveName");
            return View();
        }

        [HttpPost]
        public ActionResult Add(LeaveRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                var LeaveTypes = _leaveTypeService.GetAll();
                ViewBag.LeaveTypes = new SelectList(LeaveTypes, "LeaveTypeID", "LeaveName");
                return View(obj);
            }

            obj.EMP_ID = Convert.ToInt32(Session["Emp_ID"]);
            obj.RequestDate = DateTime.Now;

            //This Might Return 0 in case there are no corresponding record with "pending"
            obj.LeaveStatusID = _leaveStatusService.GetAll().Where(x => x.StatusName == "Pending").Select(x => x.LeaveStatusID).FirstOrDefault();
            _leaveRequestService.Insert(obj);
            return RedirectToAction("Leaves", "Leave");
        }

        public ActionResult Approve(int id)
        {
            var LeaveTypes = _leaveTypeService.GetAll();
            ViewBag.LeaveTypes = new SelectList(LeaveTypes, "LeaveTypeID", "LeaveName");
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
            return RedirectToAction("LeaveRequests", "Leave");
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