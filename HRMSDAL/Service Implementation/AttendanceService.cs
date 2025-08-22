using HRMSDAL.Helper;
using HRMSDAL.Service;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

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
                status = "Present";
            }
            else if (first != "Present" && second == "Present")
            {
                status = "Present";
            }
            else
            {
                status = "Absent";
            }
            string fullStatus = $"1st Half: {first} <br> 2nd Half: {second}";

            list.Add(new AttendanceModel
            {
                Date = date,
                Status = status,
                FullStatus = fullStatus
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

    public List<AttendanceModel> GetAttendanceByStartEndDate(int empId, DateTime startDate, DateTime endDate)
    {
        var holidayDates = GetHolidaysByStartEndDate(empId, startDate, endDate)
                           .Select(h => h.HolidayDate.Date)
                           .ToList();

        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@StartDate", startDate),
        new SqlParameter("@EndDate", endDate),
        };

        var result = DBHelper.ExecuteReader("sp_GetAttendanceByStartEndDate", CommandType.StoredProcedure, parameters);

        var attendanceRecords = new Dictionary<DateTime, AttendanceModel>();

        foreach (var row in result)
        {
            var date = Convert.ToDateTime(row["AttendanceDate"]).Date;

            attendanceRecords[date] = new AttendanceModel
            {
                Emp_ID = Convert.ToInt32(row["Emp_ID"]),
                AttendanceDate = date,
                FirstHalfStatus = row["FirstHalfStatus"] != DBNull.Value ? row["FirstHalfStatus"].ToString() : null,
                SecondHalfStatus = row["SecondHalfStatus"] != DBNull.Value ? row["SecondHalfStatus"].ToString() : null,
                ModeID = row["ModeID"] != null ? Convert.ToInt32(row["ModeID"]) : (int?)null,
                LoginTime = row["LoginTime"] != DBNull.Value ? Convert.ToDateTime(row["LoginTime"]) : (DateTime?)null,
                LogoutTime = row["LogoutTime"] != DBNull.Value ? Convert.ToDateTime(row["LogoutTime"]) : (DateTime?)null,
                FirstName = row["FirstName"] != DBNull.Value ? row["FirstName"].ToString() : string.Empty,
                MiddleName = row["MiddleName"] != null ? row["MiddleName"].ToString() : string.Empty,
                LastName = row["LastName"] != DBNull.Value ? row["LastName"].ToString() : string.Empty
            };
        }

        var attendanceList = new List<AttendanceModel>();

        for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            AttendanceModel model;

            if (attendanceRecords.ContainsKey(date))
            {
                model = attendanceRecords[date];
            }
            else
            {
                model = new AttendanceModel
                {
                    Emp_ID = empId,
                    AttendanceDate = date,
                    FirstHalfStatus = null,
                    SecondHalfStatus = null
                };
            }

            model.IsHoliday = holidayDates.Contains(date);

            model.IsWeekend = (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

            attendanceList.Add(model);
        }

        return attendanceList;
    }


    public List<HolidayModel> GetHolidaysByStartEndDate(int empId, DateTime startDate, DateTime endDate)
    {
        var holidays = new List<HolidayModel>();

        var parameters = new SqlParameter[]
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@StartDate", startDate),
        new SqlParameter("@EndDate", endDate)
        };

        var rows = DBHelper.ExecuteReader("sp_GetLocationHolidaysByDateRange", CommandType.StoredProcedure, parameters);

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
    public AttendanceModel GetAttendance(int empId, DateTime attendanceDate)
    {
        return GetAttendanceByStartEndDate(empId, attendanceDate, attendanceDate).FirstOrDefault();
    }

    public void CreateAttendanceRequest(AttendanceModel request, int loggedInRoleId)
    {
        try
        {
            var parameters = new List<SqlParameter>
        {
            new SqlParameter("@Emp_ID", request.Emp_ID),
            new SqlParameter("@AttendanceDate", request.AttendanceDate),
            new SqlParameter("@FirstHalfStatus", (object)request.FirstHalfStatus ?? DBNull.Value),
            new SqlParameter("@SecondHalfStatus", (object)request.SecondHalfStatus ?? DBNull.Value),
            new SqlParameter("@ModeID", (object)request.ModeID ?? (object)DBNull.Value),
            new SqlParameter("@LoginTime", (object)request.LoginTime ?? DBNull.Value),
            new SqlParameter("@LogoutTime", (object)request.LogoutTime ?? DBNull.Value),
            new SqlParameter("@Reason", (object)request.Reason ?? DBNull.Value),
             new SqlParameter("@LoggedInRoleId", loggedInRoleId)
        };

            // Execute stored procedure using DBHelper
            DBHelper.ExecuteNonQuery("sp_CreateAttendanceRequest", CommandType.StoredProcedure, parameters.ToArray());
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed
            throw new ApplicationException("Error while creating attendance request", ex);
        }
    }

    public List<AttendanceModel> GetAttendanceRequests(int loggedInEmpId, int loggedInRoleId)
    {
        var parameters = new SqlParameter[]
        {
        new SqlParameter("@LoggedInEmpId", loggedInEmpId),
         new SqlParameter("@LoggedInRoleId", loggedInRoleId)
        };

        var result = DBHelper.ExecuteReader("sp_GetAttendanceRequests", CommandType.StoredProcedure, parameters);

        var requestList = new List<AttendanceModel>();

        foreach (var row in result)
        {
            var model = new AttendanceModel
            {
                Emp_ID = Convert.ToInt32(row["Emp_ID"]),
                FirstName = row["FirstName"] != DBNull.Value ? row["FirstName"].ToString() : string.Empty,
                MiddleName = row["MiddleName"] != null ? row["MiddleName"].ToString() : string.Empty,
                LastName = row["LastName"] != DBNull.Value ? row["LastName"].ToString() : string.Empty,
                AttendanceDate = row["AttendanceDate"] != DBNull.Value
                     ? Convert.ToDateTime(row["AttendanceDate"]).Date
                     : DateTime.MinValue,

                CreatedOn = row["CreatedOn"] != DBNull.Value
                ? Convert.ToDateTime(row["CreatedOn"]).Date
                : DateTime.MinValue,

                Status = row["Status"] != DBNull.Value ? row["Status"].ToString() : ""
            };

            requestList.Add(model);
        }

        return requestList;
    }

   


    public AttendanceModel GetAttendanceRequestById(int empId, DateTime attendanceDate)
    {
        SqlParameter[] parameters =
        {
        new SqlParameter("@Emp_ID", empId),
        new SqlParameter("@AttendanceDate", attendanceDate.Date)
    };

        var result = DBHelper.ExecuteReader("sp_GetAttendanceRequestById", CommandType.StoredProcedure, parameters);

        var request = new List<AttendanceModel>();
        foreach (var row in result)
        {
            var model = new AttendanceModel
            {
                FirstName = row["FirstName"] != DBNull.Value ? row["FirstName"].ToString() : string.Empty,
                MiddleName = row["MiddleName"] != null ? row["MiddleName"].ToString() : string.Empty,
                LastName = row["LastName"] != DBNull.Value ? row["LastName"].ToString() : string.Empty,
                AttendanceDate = row["AttendanceDate"] != DBNull.Value
                    ? Convert.ToDateTime(row["AttendanceDate"]).Date
                    : DateTime.MinValue,
                FirstHalfStatus = row["FirstHalfStatus"] != DBNull.Value ? row["FirstHalfStatus"].ToString() : null,
                SecondHalfStatus = row["SecondHalfStatus"] != DBNull.Value ? row["SecondHalfStatus"].ToString() : null,
                ModeID = row["ModeID"] != DBNull.Value ? Convert.ToInt32(row["ModeID"]) : (int?)null,
                LoginTime = row["LoginTime"] != DBNull.Value ? Convert.ToDateTime(row["LoginTime"]) : (DateTime?)null,
                LogoutTime = row["LogoutTime"] != DBNull.Value ? Convert.ToDateTime(row["LogoutTime"]) : (DateTime?)null,
                Reason = row["Reason"] != DBNull.Value ? row["Reason"].ToString() : string.Empty,
            };

            request.Add(model);
        }

        return request.FirstOrDefault();
    }

    public void ProcessAttendanceRequest(AttendanceModel model, string action)
    {
        string status = action == "Approve" ? "Approved" : "Rejected";

 
        if (status == "Approved")
        {
            SqlParameter[] attendanceParams =
            {
            new SqlParameter("@Emp_ID", model.Emp_ID),
            new SqlParameter("@AttendanceDate", model.AttendanceDate),
            new SqlParameter("@FirstHalfStatus", model.FirstHalfStatus ?? (object)DBNull.Value),
            new SqlParameter("@SecondHalfStatus", model.SecondHalfStatus ?? (object)DBNull.Value),
            new SqlParameter("@ModeID", model.ModeID ?? (object)DBNull.Value),
            new SqlParameter("@LoginTime", (object)model.LoginTime ?? DBNull.Value),
            new SqlParameter("@LogoutTime", (object)model.LogoutTime ?? DBNull.Value)
        };

            DBHelper.ExecuteNonQuery("sp_UpdateAttendanceFromRequest", CommandType.StoredProcedure, attendanceParams);
        }

        SqlParameter[] requestParams =
        {
        new SqlParameter("@Emp_ID", model.Emp_ID),
        new SqlParameter("@AttendanceDate", model.AttendanceDate),
        new SqlParameter("@Comment", model.Comment),
        new SqlParameter("@Status", status)
    };

        DBHelper.ExecuteNonQuery("sp_UpdateAttendanceRequestStatus", CommandType.StoredProcedure, requestParams);
    }


}

