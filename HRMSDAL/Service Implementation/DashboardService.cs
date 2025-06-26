using HRMSDAL.Helper;
using HRMSDAL.Service;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HRMSDAL.Service_Implementation
{
    public class DashboardService : IDashboardService
    {
        public NavbarModel GetNavbarData(int empId, int roleId)
        {
            NavbarModel model = new NavbarModel();

            var employeeData = DBHelper.ExecuteReader(
                "usp_GetNavbarInfoByEmpID",
                CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@Emp_ID", empId) }
            );

            if (employeeData.Count > 0)
            {
                var row = employeeData[0];

                model.EmployeeID = row["EmployeeID"]?.ToString();
                model.FirstName = row["FirstName"]?.ToString();
                model.MiddleName = row["MiddleName"]?.ToString();
                model.LastName = row["LastName"]?.ToString();
                model.ProfileImagePath = string.IsNullOrEmpty(row["ProfileImagePath"]?.ToString())
                    ? "/Content/Images/default.png"
                    : row["ProfileImagePath"]?.ToString();
                model.RoleName = row["RoleName"]?.ToString();
            }
            return model;
        }
    }
}








