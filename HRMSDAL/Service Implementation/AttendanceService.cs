using HRMSDAL.Helper;
using HRMSDAL.Service;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class AttendanceService : IAttendanceService
{
    public void MarkLoginTime(int empId)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@Today", DateTime.Today),
        new SqlParameter("@LoginTime", DateTime.Now),
        new SqlParameter("@ModeID", 1) 
        };

        DBHelper.ExecuteNonQuery("sp_MarkLoginTime", CommandType.StoredProcedure, parameters);
    }


    public void MarkLogoutTime(int empId)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@Today", DateTime.Today),
        new SqlParameter("@LogoutTime", DateTime.Now)
        };

        DBHelper.ExecuteNonQuery("sp_MarkLogoutTime", CommandType.StoredProcedure, parameters);
    }


    public List<AttendanceModel> GetAttendanceCalendar(int empId, int year, int month)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@Month", month),
        new SqlParameter("@Year", year)
        };

        var rows = DBHelper.ExecuteReader("sp_GetAttendanceCalendar", CommandType.StoredProcedure, parameters);

        List<AttendanceModel> list = new List<AttendanceModel>();

        foreach (var row in rows)
        {
            var date = Convert.ToDateTime(row["AttendanceDate"]);
            var first = row["FirstHalfStatus"]?.ToString();
            var second = row["SecondHalfStatus"]?.ToString();
            var status = (first == "Present" || second == "Present") ? "Present" : "Absent";

            list.Add(new AttendanceModel
            {
                Date = date,
                Status = status
            });
        }

        return list;
    }

}

