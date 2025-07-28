using HRMSDAL.Helper;
using HRMSDAL.Service;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSDAL.Service_Implementation
{
    public class EmployeeProfileService : IEmployeeProfileService
    {

        public EmployeeProfileModel GetProfile(int empId)
        {
            EmployeeProfileModel model = new EmployeeProfileModel();


            var result = DBHelper.ExecuteReader(
                    "usp_GetMyProfileData",
                    CommandType.StoredProcedure,
                    new SqlParameter[] { new SqlParameter("@Emp_ID", empId) }
                );

            if (result.Count > 0)
            {
                var row = result[0];

                model.FirstName = row["FirstName"]?.ToString();
                model.MiddleName = row["MiddleName"]?.ToString();
                model.LastName = row["LastName"]?.ToString();
                model.Email = row["Email"]?.ToString();
                model.ContactNumber = Convert.ToInt64(row["ContactNumber"]);
                model.ProfileImagePath = row["ProfileImagePath"] as byte[];
                model.Password = row["Password"]?.ToString();
                model.Address = row["Address"]?.ToString();
                model.StateName = row["StateName"]?.ToString();
                model.CityName = row["CityName"]?.ToString();
                model.CountryName = row["CountryName"]?.ToString();
                model.RoleName = row["RoleName"]?.ToString();
            }
            return model;

        }



        public bool ChangePassword(int empId, string currentPassword, string newPassword)
        {

            var param = new SqlParameter[] {
                new SqlParameter("@Emp_ID", empId),
                new SqlParameter("@CurrentPassword", currentPassword),
                new SqlParameter("@NewPassword", newPassword)
            };

            int result = DBHelper.ExecuteNonQuery("sp_ChangePassword", CommandType.StoredProcedure, param);
            return result > 0;
        }
    }
}
