using HRMSModels;
using HRMSDAL.Service;
using HRMSUtility;
using System;
using System.Web;
using System.Web.Mvc;

namespace HRMSProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IAttendanceService _attendanceService;

        public LoginController(ILoginService loginService, IAttendanceService attendanceService)
        {
            _loginService = loginService;
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            LoginModel model = new LoginModel();

            HttpCookie cookie = Request.Cookies["LoginCookie"];
            if (CommonHelper.AreNotEqual(cookie, null))
            {
                model.Email = cookie["Email"];
                model.Password = cookie["Password"];
                model.RememberMe = true;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {          
                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    ModelState.AddModelError("Email", "Please enter email");
                    return View(model);
                }

                LoginModel user = _loginService.GetUserByEmail(model.Email);
                if (CommonHelper.AreEqual(user, null))
                {
                    ModelState.AddModelError("Email", "Invalid Credential");
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError("Password", "Please enter password");
                    return View(model);
                }

                string decodedPassword = Base64Helper.Decode(user.Password);
                if (decodedPassword != model.Password)
                {
                    ModelState.AddModelError("Password", "Invalid password");
                    return View(model);
                }

                Session["Emp_ID"] = user.Emp_ID;
                Session["RoleID"] = user.RoleID;
               

            
                if (model.RememberMe)
                {
                    HttpCookie cookie = new HttpCookie("LoginCookie");
                    cookie["Email"] = model.Email;
                    cookie["Password"] = model.Password;
                    cookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookie);
                }

                int empId = user.Emp_ID;
                DateTime today = DateTime.Today;

                var attendance = _attendanceService.GetAttendanceByDate(empId, today);
                int? modeId = attendance != null ? attendance.ModeID : null;


            _attendanceService.MarkLoginTime(empId, modeId);


                return RedirectToAction("Index", "Dashboard");
            
        }

        public ActionResult Logout()
        {
         
            if (Session["Emp_ID"] != null)
            {
                int empId = Convert.ToInt32(Session["Emp_ID"]);

                _attendanceService.MarkLogoutTime(empId);
            }

       
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Login");
        }
    }
}


