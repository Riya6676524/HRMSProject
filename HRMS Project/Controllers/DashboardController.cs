using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HRMSProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMenuService _menuService;
        private readonly IRoleMenuService _roleMenuService;

        public DashboardController(IDashboardService dashboardService, IMenuService menuService, IRoleMenuService roleMenuService)
        {
            _dashboardService = dashboardService;
            _menuService = menuService;
            _roleMenuService = roleMenuService;
        }

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetLeaveChartData()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            var summary = _dashboardService.GetLeaveSummary(empId);
            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //[OutputCache(Duration = 69)]

        public JsonResult GetNavbarData()
        {
            if (Session["Emp_ID"] == null || Session["RoleID"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            int empId = (int)Session["Emp_ID"];
            int roleId = (int)Session["RoleID"];

            var result = _dashboardService.GetNavbarData(empId, roleId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //[OutputCache(Duration =69)]
        public JsonResult GetMenus()
        {
            int roleId = 1;

            var roleMenus = _roleMenuService.GetAll()
                              .Where(rm => rm.RoleID == roleId)
                              .ToList();

            var allMenus = _menuService.GetAll();

            var finalMenus = allMenus
                .Where(m => roleMenus.Any(rm => rm.MenuID == m.MenuID))
                .OrderBy(m => m.ParentMenuID)
                .ThenBy(m => m.DisplayOrder)
                .ToList();

            return Json(finalMenus, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetDashboardData()
        {
            if (Session["Emp_ID"] == null || Session["RoleID"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            int empId = (int)Session["Emp_ID"];
            int roleId = (int)Session["RoleID"];

            var empData = _dashboardService.GetNavbarData(empId, roleId);

            var dashboardData = new
            {
                FirstName = empData.FirstName

            };

            return Json(dashboardData, JsonRequestBehavior.AllowGet);
        }
    }
}

