using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSDAL.Helper;
using HRMSDAL.Service;
using HRMSModels;

namespace HRMSDAL.Service_Implementation
{
    public class LeaveBalanceService : GenericService<LeaveBalanceModel>, ILeaveBalanceService
    {
        protected override string TableName => "LeaveBalance";

        protected override string PrimaryKeyColumn => throw new NotImplementedException("This table does not contain a single primary key column");


        private void RefreshBalanceMonths()
        {
            string query = "usp_InitBalanceMonths";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ForMonth", DateTime.Now)
            };
            DBHelper.ExecuteNonQuery(query, System.Data.CommandType.StoredProcedure, parameters);
        }

        public void CreditMonthlyLeaves()
        {
            RefreshBalanceMonths();
            string query = "usp_CreditLeaves";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ForMonth", DateTime.Now),
                new SqlParameter("@Amount",2)
            };
            DBHelper.ExecuteNonQuery(query, System.Data.CommandType.StoredProcedure,parameters);
        }

        public LeaveBalanceModel GetByIdandMonth(int empID, DateTime month)
        {
            string query = $"SELECT * FROM {TableName} WHERE EMP_ID = @EMP_ID and MONTH(ForMonth)=MONTH(@ForMonth) and YEAR(ForMonth) = YEAR(@ForMonth)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@EMP_ID", empID),
                new SqlParameter("@ForMonth", month)
            };
            var result = DBHelper.ExecuteReader(query, System.Data.CommandType.Text, parameters);
            return MapDictionaryToEntity(result.First());
        }

        List<LeaveBalanceModel> ILeaveBalanceService.GetById(int empID)
        {
            string query = $"SELECT * FROM {TableName} WHERE EMP_ID = @EMP_ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@EMP_ID", empID)
            };
            var result = DBHelper.ExecuteReader(query, System.Data.CommandType.Text, parameters);
            return result.Select(x => MapDictionaryToEntity(x)).ToList();
        }


        public override LeaveBalanceModel GetById(int id)
        {
            throw new NotImplementedException("This method is not valid for the given service class.");
        }
    }
}
