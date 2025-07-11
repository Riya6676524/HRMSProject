using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;
using HRMSUtility;

namespace HRMS.Controllers
{
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

        [HttpGet]
        public JsonResult CalculateLeaveDays(DateTime fromDate  , DateTime toDate , bool fromSecondHalf =false, bool uptoFirstHalf = false)
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

            obj.EMP_ID = 1;
            obj.RequestDate = DateTime.Now;

            //This Might Return 0 in case there are no corresponding record with "pending"
            obj.LeaveStatusID = _leaveStatusService.GetAll().Where(x => x.StatusName == "Pending").Select(x => x.LeaveStatusID).FirstOrDefault();
            _leaveRequestService.Insert(obj);
            return RedirectToAction("Leaves", "Leave");
        }

        [HttpPost]
        public JsonResult LeaveRequest(LeaveRequestModel obj)
        {
            _leaveRequestService.Insert(obj);
            return Json(new { success = true });
        }

        public ActionResult Approve(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Approve(int id, string Action)
        {
            LeaveRequestModel toBeApprovedModel = _leaveRequestService.GetById(id);
            LeaveStatusModel curStatus = _leaveStatusService.GetById(toBeApprovedModel.LeaveStatusID);
            List<LeaveStatusModel> allStatuses = _leaveStatusService.GetAll().ToList();
            if (curStatus.StatusName == "PENDING")
            {
                if (Action == "Approve")
                {
                    LeaveStatusModel approveStatus = allStatuses.FirstOrDefault(x => x.StatusName == "Approved");
                    toBeApprovedModel.LeaveStatusID = approveStatus.LeaveStatusID;
                    _leaveRequestService.Update(toBeApprovedModel);
                }
                else if (Action == "DENY")
                {
                    LeaveStatusModel approveStatus = allStatuses.FirstOrDefault(x => x.StatusName == "CANCELLED");
                    toBeApprovedModel.LeaveStatusID = approveStatus.LeaveStatusID;
                    _leaveRequestService.Update(toBeApprovedModel);
                }
            }
            return View();
        }
    }
}
