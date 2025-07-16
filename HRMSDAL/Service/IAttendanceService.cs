using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSDAL.Service
{
    public interface IAttendanceService
    {
        void MarkLoginTime(int empId);
        void MarkLogoutTime(int empId);
        List<AttendanceModel> GetAttendanceCalendar(int empId, int year, int month);

    }
}
