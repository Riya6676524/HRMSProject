using HRMSDAL.Service;
using System.Web.Mvc;

namespace HRMSProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public ActionResult Index()
        {
            int empId = (int)Session["Emp_ID"];
            int roleId = (int)Session["RoleID"];

            var model = _dashboardService.GetNavbarData(empId, roleId);
            return View(model);
        }
    }
}
