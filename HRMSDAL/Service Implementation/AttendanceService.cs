using HRMSModels;
using HRMSDAL.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class AttendanceService
{
    public bool MarkAttendance(AttendanceModel model)
    {
        try
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Emp_ID", model.Emp_ID),
                new SqlParameter("@AttendanceDate", model.AttendanceDate),
                new SqlParameter("@FirstHalfStatus", model.FirstHalfStatus),
                new SqlParameter("@SecondHalfStatus", model.SecondHalfStatus),
                new SqlParameter("@ModeID", model.ModeID)
            };

            int result = DBHelper.ExecuteNonQuery("sp_MarkAttendance", CommandType.StoredProcedure, parameters);
            return result > 0;
        }
        catch (Exception)
        {
            // log exception
            return false;
        }
    }

    public List<AttendanceModel> GetAttendanceHistory(int empId, int month, int year)
    {
        List<AttendanceModel> attendanceList = new List<AttendanceModel>();

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@Emp_ID", empId),
            new SqlParameter("@Month", month),
            new SqlParameter("@Year", year)
        };

        var result = DBHelper.ExecuteReader("sp_GetAttendanceHistory", CommandType.StoredProcedure, parameters);

        foreach (var row in result)
        {
            attendanceList.Add(new AttendanceModel
            {
                AttendanceDate = Convert.ToDateTime(row["AttendanceDate"]),
                FirstHalfStatus = row["FirstHalfStatus"]?.ToString(),
                SecondHalfStatus = row["SecondHalfStatus"]?.ToString()
            });
        }

        return attendanceList;
    }
}
