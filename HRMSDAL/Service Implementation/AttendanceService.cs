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
    public void MarkLoginTime(int empId, int modeId)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@Today", DateTime.Today),
        new SqlParameter("@LoginTime", DateTime.Now),
        new SqlParameter("@ModeID", modeId) 
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

    public int GetModeIdByName(string modeName)
    {
        SqlParameter[] parameters = new SqlParameter[]
        {
        new SqlParameter("@ModeName", modeName)
        };

        object result = DBHelper.ExecuteScalar("sp_GetModeIDByName", CommandType.StoredProcedure, parameters);
        return result != null ? Convert.ToInt32(result) : 0;
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

    public List<HolidayModel> GetLocationHoliday(int empId, int month, int year)
    {
        List<HolidayModel> holidays = new List<HolidayModel>();

        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@Month", month),
        new SqlParameter("@Year", year)
        };

        var rows = DBHelper.ExecuteReader("sp_GetLocationHolidays", CommandType.StoredProcedure, parameters);

        foreach (var row in rows)
        {
            holidays.Add(new HolidayModel
            {
                HolidayDate = row["HolidayDate"] != null ? Convert.ToDateTime(row["HolidayDate"]) : DateTime.MinValue,
                HolidayName = row["HolidayName"]?.ToString()
            });
        }

        return holidays;
    }



}

