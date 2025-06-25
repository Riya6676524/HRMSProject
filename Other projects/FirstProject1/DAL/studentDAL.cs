using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using FirstProject1.Models;

namespace FirstProject1.DAL
{
    public class studentDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public int AddStudent(student stud)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO students (name, email, phone, address, uerid) VALUES (@name, @email, @phone, @address, @uerid)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", stud.name);
                cmd.Parameters.AddWithValue("@email", stud.email);
                cmd.Parameters.AddWithValue("@phone", stud.phone);
                cmd.Parameters.AddWithValue("@address", stud.address);
                cmd.Parameters.AddWithValue("@uerid", stud.uerid);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public List<student> GetAllStudents()
        {
            List<student> students = new List<student>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM students";  // Correct table name

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    student st = new student()
                    {
                        studentid = Convert.ToInt32(reader["studentid"]),
                        name = reader["name"].ToString(),
                        email = reader["email"].ToString(),
                        phone = reader["phone"].ToString(),
                        address = reader["address"].ToString(),
                        uerid = Convert.ToInt32(reader["uerid"])  
                    };

                    students.Add(st);
                }
            }

            return students;
        }

    }


}
