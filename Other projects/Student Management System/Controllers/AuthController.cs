using Student_Management_System.Models;
using System.Web.Mvc;

namespace Student_Management_System.Controllers
{
    public class AuthController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(user user)
        {
            // Add login logic here
            return RedirectToAction("Index", "Home");
        }

        // GET: Signup
        public ActionResult Signup()
        {
            return View();
        }

        // POST: Signup
        [HttpPost]
        public ActionResult Signup(user user)
        {
            // Add signup logic here
            return RedirectToAction("Login");
        }
    }
}
