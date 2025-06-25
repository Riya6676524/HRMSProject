using FirstProject1.DAL;
using FirstProject1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstProject1.Controllers
{
    public class UserController : Controller
    {
            UserDAL userDAL = new UserDAL();

            [HttpGet]
            public ActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public ActionResult Login(User user)
            {
                var loggedInUser = userDAL.ValidateUser(user.username, user.password);
                if (loggedInUser != null)
                {
                    Session["username"] = loggedInUser.username;
                Session["username"] = loggedInUser.password;
                Session["role"] = loggedInUser.role;

                    if (loggedInUser.role == 1)
                        return RedirectToAction("list", "Student");
                    else
                        return RedirectToAction("StudentDashboard", "Student");
                }
                else
                {
                    ViewBag.Message = "Invalid credentials!";
                    return View();
                }
            }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

    }

}