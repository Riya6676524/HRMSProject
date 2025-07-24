using HRMSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSDAL.Service
{
    public interface IDashboardService
    {
        NavbarModel GetNavbarData(int empId);
        LeaveSummaryModel GetLeaveSummary(int empId);
    }
}
