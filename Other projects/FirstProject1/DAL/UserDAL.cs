using FirstProject1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FirstProject1.DAL
{
    public class UserDAL
    {
        public User ValidateUser(string username, string password)
        {
            User user = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM users WHERE username=@username AND password=@password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User()
                    {
                        userid = Convert.ToInt32(reader["userid"]),
                        username = reader["username"].ToString(),
                        role = Convert.ToInt32(reader["role"])
                    };
                }
            }

            return user;
        }

    }
}