using Data_Access_Layer.DAL;
using student_model.Models;
using System;
using System.Web.Mvc;

namespace student_management.Controllers
{
    public class AuthController : Controller
    {
        userDAL userDAL = new userDAL();

        //get: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        //post: Auth/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = userDAL.AuthenticateUser(username, password);
            if (user != null)
            {
                Session["UserId"] = user.userid;
                Session["Role"] = user.role;

                if (user.role == 1)
                    return RedirectToAction("Index", "Student");
                else
                    return RedirectToAction("Index", "Course");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        //get: Auth/Signup
        public ActionResult Signup()
        {
            return View();
        }

        //post: Auth/Signup
        [HttpPost]
        public ActionResult Signup(user newUser)
        {

            int roleId = 0;
            if (newUser.rolename == "Admin")
                roleId = 1;
            else if (newUser.rolename == "Student")
                roleId = 2;

            newUser.role = roleId;

            //Check if user already exists
            if (userDAL.IsUserExists(newUser.username))
            {
                ViewBag.Message = "Username already exists. Please login instead.";
                return View();
            }

            bool result = userDAL.InsertUser(newUser);

            if (result)
            {
                ViewBag.Message = "User created successfully.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = "Error in user creation.";
                return View();
            }
        }

    }
}




