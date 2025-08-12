using HRMSDAL.Helper;
using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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

            LeaveRequestService obj = new LeaveRequestService();
            int currentMonth = DateTime.Now.Month;
            int LeaveTaken = (int)obj.GetLeavesByEmp_ID(empId).Where(item => item.StartDate.Month <= DateTime.Now.Month && item.EndDate.Month <= DateTime.Now.Month).Sum(item => item.TotalDays);
            int TotalAvailableLeave = currentMonth * 2 - LeaveTaken;

            return new LeaveSummaryModel
            {

                LeaveTaken = LeaveTaken,
                TotalAvailable = TotalAvailableLeave
            };
        }
    }
}







