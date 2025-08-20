using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;

namespace HRMSDAL.Service
{
    public interface ILeaveBalanceService 
    {
        List<LeaveBalanceModel> GetAll();
        LeaveBalanceModel GetById(int id);
        LeaveBalanceModel GetByIdandMonth(int id, DateTime month);
        void Insert (LeaveBalanceModel entity);
        void Update (LeaveBalanceModel entity);
        void CreditMonthlyLeaves();
        List<LeaveBalanceModel> GetAllMonthByID(int empID);
        void UpdateFinalBalanceNSync(int EMP_ID, DateTime ForMonth, float NewBalance);

        void RefreshBalanceMonths();
    }
}
