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
    public void MarkLoginTime(int empId, int? modeId)
    {
        var today = DateTime.Today;

        if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
        {
            return; 
        }

        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@ModeID", (object)modeId ?? DBNull.Value)
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

    public void UpdateMode(int empId, int modeId)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@Today", DateTime.Today),
        new SqlParameter("@ModeID", modeId)
        };

        DBHelper.ExecuteNonQuery("sp_UpdateAttendanceMode", CommandType.StoredProcedure, parameters);
    }

    public string GetTodayModeName(int empId)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@Today", DateTime.Today)
        };

        object result = DBHelper.ExecuteScalar("sp_GetTodayModeName", CommandType.StoredProcedure, parameters);
        return result?.ToString();
    }


    public AttendanceModel GetAttendanceByDate(int empId, DateTime date)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@Date", date.Date) 
        };

        var result = DBHelper.ExecuteReader("sp_GetAttendanceByDate", CommandType.StoredProcedure, parameters);
        if (result.Count > 0)
        {
            var row = result[0];
            return new AttendanceModel
            {
                Emp_ID = Convert.ToInt32(row["Emp_ID"]),
                AttendanceDate = Convert.ToDateTime(row["AttendanceDate"]),
                FirstHalfStatus = row["FirstHalfStatus"]?.ToString(),
                SecondHalfStatus = row["SecondHalfStatus"]?.ToString(),
                ModeID = row["ModeID"] != null ? Convert.ToInt32(row["ModeID"]) : (int?)null,
                LoginTime = row["LoginTime"] != DBNull.Value ? Convert.ToDateTime(row["LoginTime"]) : (DateTime?)null,
                LogoutTime = row["LogoutTime"] != DBNull.Value ? Convert.ToDateTime(row["LogoutTime"]) : (DateTime?)null
            };
        }

        return null;
    }


    public List<AttendanceModel> GetAttendanceCalendar(int empId)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId)
        };

        var rows = DBHelper.ExecuteReader("sp_GetAttendanceCalendar", CommandType.StoredProcedure, parameters);

        List<AttendanceModel> list = new List<AttendanceModel>();

        foreach (var row in rows)
        {
            var date = Convert.ToDateTime(row["AttendanceDate"]);
            var first = row["FirstHalfStatus"]?.ToString();
            var second = row["SecondHalfStatus"]?.ToString();
            string status = "";

            if (first == "Present" && second == "Present")
            {
                status = "Present";
            }
            else if (first == "Present" && second != "Present")
            {
                status = "1stHalf: Present";
            }
            else if (first != "Present" && second == "Present")
            {
                status = "2ndHalf: Present";
            }
            else
            {
                status = "Absent";
            }

            list.Add(new AttendanceModel
            {
                Date = date,
                Status = status
            });
        }

        return list;
    }

    public List<HolidayModel> GetLocationHoliday(int empId)
    {
        List<HolidayModel> holidays = new List<HolidayModel>();

        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId)
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

