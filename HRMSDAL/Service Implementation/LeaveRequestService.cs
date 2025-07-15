using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;
using HRMSDAL.Helper;
using System.Data;
using System.Data.SqlClient;

namespace HRMSDAL.Service_Implementation
{
    public class LeaveRequestService : GenericService<LeaveRequestModel>, ILeaveRequestService
    {
        protected override string TableName => "LeaveRequest";
        protected override string PrimaryKeyColumn => "RequestID";

        public List<LeaveRequestModel> GetLeavesByEmp_ID(int empId)
        {
            List<LeaveRequestModel> all = GetAll();
            return all.Where(x => x.EMP_ID == empId).ToList();
        }

        public List<LeaveRequestModel> GetLeavesByManager(int managerID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ManagerID", managerID)
            };
            var res = DBHelper.ExecuteReader("usp_getLeavesByManager", CommandType.StoredProcedure,parameters);
            return res.Select(x => MapDictionaryToEntity(x)).ToList();
        }
    }
}



