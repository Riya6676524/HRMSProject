using HRMSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSDAL.Service
{
    public interface IAttendanceService
    {
        void MarkLoginTime(int empId, int? modeId);
        void MarkLogoutTime(int empId);
        List<AttendanceModel> GetAttendanceCalendar(int empId);
        int GetModeIdByName(string modeName);
        List<HolidayModel> GetLocationHoliday(int empId);
        void UpdateMode(int empId, int modeId);
        string GetTodayModeName(int empId);
        AttendanceModel GetAttendanceByDate(int empId, DateTime date);
        List<AttendanceModel> GetAttendanceByStartEndDate(int empId, DateTime startDate, DateTime endDate);
        AttendanceModel GetAttendance(int empId, DateTime attendanceDate);
        void CreateAttendanceRequest(AttendanceModel request);
        List<HolidayModel> GetHolidaysByStartEndDate(int empId, DateTime startDate, DateTime endDate);
        List<AttendanceModel> GetAttendanceRequests(int loggedInEmpId);
        bool ApproveAttendanceRequest(int empId, DateTime attendanceDate, string firstHalfStatus,
                                       string secondHalfStatus, int? modeId,
                                       DateTime? loginTime, DateTime? logoutTime, string comment);
        bool RejectAttendanceRequest(int empId, DateTime attendanceDate, string comment);
        AttendanceModel GetAttendanceRequestById(int empId, DateTime attendanceDate);

    }
}
