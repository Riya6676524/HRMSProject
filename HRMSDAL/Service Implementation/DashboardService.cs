using HRMSDAL.Helper;
using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace HRMSDAL.Service_Implementation
{
    public class DashboardService : IDashboardService
    {
        public NavbarModel GetNavbarData(int empId)
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
                model.ProfileImagePath = row["ProfileImagePath"] as byte[];
                model.RoleName = row["RoleName"]?.ToString();
            }
            return model;
        }

      
        public LeaveSummaryModel GetLeaveSummary(int empId)
        {
            LeaveSummaryModel summary = new LeaveSummaryModel();

            var leaveData = DBHelper.ExecuteReader(
                "sp_GetLeaveSummary",
                CommandType.StoredProcedure,
                new SqlParameter[]
                {
            new SqlParameter("@Emp_ID", empId)
                }
            );

            if (leaveData.Count > 0)
            {
                var row = leaveData[0];

                summary.TotalOpening = row["TotalOpening"] != DBNull.Value ? Convert.ToDecimal(row["TotalOpening"]) : 0;
                summary.TotalCredited = row["TotalCredited"] != DBNull.Value ? Convert.ToDecimal(row["TotalCredited"]) : 0;
                summary.TotalClosing = row["TotalClosing"] != DBNull.Value ? Convert.ToDecimal(row["TotalClosing"]) : 0;
            }

            return new LeaveSummaryModel
            {

                LeaveTaken = (summary.TotalOpening + summary.TotalCredited) - summary.TotalClosing,
                TotalAvailable = summary.TotalClosing
            };
        }


    }
}







