using student_model.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Data_Access_Layer.DAL
{
    public class studentDAL
    {
            private string conStr = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public List<student> GetAllStudents()
            {
                List<student> students = new List<student>();
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "SELECT * FROM students";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        students.Add(new student
                        {
                            studentid = (int)reader["studentid"],
                            name = reader["name"].ToString(),
                            email = reader["email"].ToString(),
                            phone = reader["phone"].ToString(),
                            address = reader["address"].ToString(),
                            Role = (int)reader["role"],
                     
                        });
                    }
                }
                return students;
            }
        }
    }

