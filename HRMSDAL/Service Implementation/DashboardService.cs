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
    
            public LeaveSummaryModel GetLeaveSummary(int empId)
            {
               
                int currentMonth = DateTime.Now.Month;
                int totalEntitled = currentMonth * 2;
                double leaveTaken = 1;
                double carryForward = 2; //ye databse se aayega as its yearly carryForward leave 
                                         // we need cary forward leave table which contain the availableleave of previous year 



            var result = DBHelper.ExecuteScalar(
             "GetApprovedLeaveTaken",
             CommandType.StoredProcedure,
             new SqlParameter[] { new SqlParameter("@Emp_ID", empId) }
         );

                if (result != null && result != DBNull.Value)
                {
                    leaveTaken = Convert.ToDouble(result);
                }
              
                double totalAvailable = Convert.ToDouble(totalEntitled-leaveTaken+carryForward);
                
                return new LeaveSummaryModel
                {
                    TotalAvailable = totalAvailable,
                    LeaveTaken = Convert.ToInt32(leaveTaken)
                };
            }
        }
    }









