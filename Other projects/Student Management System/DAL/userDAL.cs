using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Student_Management_System.DAL
{
    public class userDAL
    {
        string conStr = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public void AddUser(user u)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "INSERT INTO users(username, password, role) VALUES(@username, @password, @role)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", u.username);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@role", u.role);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public user Login(string username, string password)
        {
            user u = null;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM users WHERE username=@username AND password=@password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    u = new user()
                    {
                        userid = (int)reader["userid"],
                        username = reader["username"].ToString(),
                        password = reader["password"].ToString(),
                        role = reader["role"].ToString()
                    };
                }
            }
            return u;
        }
    }
}