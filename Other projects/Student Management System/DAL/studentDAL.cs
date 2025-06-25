using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Student_Management_System.DAL
{
    public class studentDAL
        {
            private string connectionString = "MyDbConnection";

            public List<student> GetAllStudents()
            {
                List<student> students = new List<student>();
                using (SqlConnection con = new SqlConnection(connectionString))
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
                            uerid = (int)reader["uerid"]
                        });
                    }
                }
                return students;
            }
        }
    }

