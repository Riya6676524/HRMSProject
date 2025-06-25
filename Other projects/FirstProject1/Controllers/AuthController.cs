using System.Web.Mvc;
using FirstProject1.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace FirstProject1.Controllers
{
    public class AuthController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserModel user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@role", user.Role);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            ViewBag.Message = "Registration successful!";
            return RedirectToAction("Login", "user");

        }
    }
}
