using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HRMSDAL.Service;
using HRMSModels;
using HRMSUtility;

namespace HRMS.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeaveRequestService _leaveService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveStatusService _leaveStatusService;

        public LeaveController(ILeaveRequestService leaveService, ILeaveTypeService leaveTypeService, ILeaveStatusService leaveStatusService)
        {
            _leaveService = leaveService;
            _leaveTypeService = leaveTypeService;
            _leaveStatusService = leaveStatusService;
        }

        public ActionResult TestView()
        {
            return View();
        }

        //[HttpPost]
        //public JsonResult CalculateLeaveDays(DateTime fromDate, DateTime toDate)
        //{

        //    int result = DateUtility.CalculateLeaveDays(fromDate, toDate);
        //    return Json(new { leaveDays = result });
        //}

        public ActionResult Add()
        {
            var LeaveTypes = _leaveTypeService.GetAll();
            ViewBag.LeaveTypes = new SelectList(LeaveTypes, "LeaveTypeID", "LeaveName");
            return View();
        }

        [HttpPost]
        public JsonResult LeaveRequest(LeaveRequestModel obj)
        {
            _leaveService.Insert(obj);
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult LeaveApprove(string id)
        {
            int requestId = int.Parse(id);
            var leaveRequest = _leaveService.GetById(requestId);

            int approverId = Convert.ToInt32(Session["UserID"]);
            leaveRequest.LeaveStatusID = 1;
            leaveRequest.ApproverID = approverId;
            leaveRequest.ApproverDate = DateTime.Now;

            _leaveService.Update(leaveRequest);

            return Json(new { success = true });
        }
    }
}
