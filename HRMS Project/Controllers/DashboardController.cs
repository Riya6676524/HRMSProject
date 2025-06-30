using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;
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

        [HttpGet]
        public JsonResult GetNavbarData()
        {
            int empId = 1;
            int roleId = 1;
            var result = _dashboardService.GetNavbarData(empId, roleId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
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
    }
}
