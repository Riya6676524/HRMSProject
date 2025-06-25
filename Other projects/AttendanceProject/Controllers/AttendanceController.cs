using AttendanceProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace AttendanceProject.Controllers
{
    public class AttendanceController : Controller
    {
        public ActionResult Create()
        {
            var model = new AttendanceModel
            {
                AttendanceDate = DateTime.Today,
                IsAdmin = false, // Later use session or role
                UserID = 0
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitAttendance(AttendanceModel model)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", model.UserID);
                cmd.Parameters.AddWithValue("@AttendanceDate", model.AttendanceDate);
                cmd.Parameters.AddWithValue("@AttendanceType", model.AttendanceType);
                cmd.Parameters.AddWithValue("@CreatedBy", model.UserID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            ViewBag.Message = "Attendance marked!";
            return RedirectToAction("Create");
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public JsonResult GetAttendanceEvents()
        {
            List<object> events = new List<object>();
            int userId = 0; // demo

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT AttendanceDate,AttendanceType FROM Attendance WHERE UserID = @UserID", con);
                cmd.Parameters.AddWithValue("@UserID", userId);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string attendanceType = dr["AttendanceType"].ToString();
                    string color = "green";
                    string title = "";

                    if (attendanceType == "Full Day")
                    {
                        title = "Full Day Present";
                        color = "green";
                    }
                    else if (attendanceType == "First Half")
                    {
                        title = "First Half Present";
                        color = "#87CEFA"; // light blue
                    }
                    else if (attendanceType == "Second Half")
                    {
                        title = "Second Half Present";
                        color = "#FFA07A"; // light salmon
                    }
                    else
                    {
                        title = "Unknown Attendance";
                        color = "gray";
                    }

                    events.Add(new
                    {
                        title = title,
                        start = Convert.ToDateTime(dr["AttendanceDate"]).ToString("yyyy-MM-dd"),
                        color = color
                    });

                }
                dr.Close();



                SqlCommand holidayCmd = new SqlCommand("SELECT HolidayDate, Description FROM Holidays", con);
                SqlDataReader holidayDr = holidayCmd.ExecuteReader();
                while (holidayDr.Read())
                {
                    events.Add(new
                    {
                        title = holidayDr["Description"].ToString(),
                        start = Convert.ToDateTime(holidayDr["HolidayDate"]).ToString("yyyy-MM-dd"),
                        color = "gray"
                    });
                }
                holidayDr.Close();
            }

            return Json(events, JsonRequestBehavior.AllowGet);
        }
    }
}

