using student_model.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Data_Access_Layer.DAL
{
    public class userDAL
    {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

            public user AuthenticateUser(string username, string password)
            {
                user userObj=null;

                using (SqlConnection con = new SqlConnection(conStr))
                {
             
                string query = "SELECT * FROM [users] WHERE username = @username AND password = @password";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userObj = new user
                        {
                            userid = Convert.ToInt32(reader["userid"]),
                            username = reader["username"].ToString(),
                            password = reader["password"].ToString(),
                            role = (int)reader["role"],
                            
                        };
                    }
                }

                return userObj;
            }

            public bool RegisterUser(user user)
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "INSERT INTO [users] (username, password, role) VALUES (@username, @password, @Roleid)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@password", user.password);
                    cmd.Parameters.AddWithValue("@Roleid", user.role);

                con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        public bool IsUserExists(string username)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM users WHERE username = @username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public bool InsertUser(user newUser)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                string query = "INSERT INTO [users] (username, password, role) VALUES (@username, @password, @Roleid)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", newUser.username);
                cmd.Parameters.AddWithValue("@password", newUser.password);
                cmd.Parameters.AddWithValue("@Roleid", newUser.role);
                

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected > 0;
            }
        }


    }
}

   