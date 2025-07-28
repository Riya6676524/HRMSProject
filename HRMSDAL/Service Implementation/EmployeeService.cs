using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Helper;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
public class EmployeeService : GenericService<EmployeeModel>, IEmployeeService
    {
        protected override string TableName => "Employee";
        protected override string PrimaryKeyColumn => "EMP_ID";
        
            public string GetNextAvailableEmployeeId()
        {
            string query = "SELECT TOP 1 EmployeeID FROM Employee WHERE EmployeeID LIKE 'Optimum-%' ORDER BY EmployeeID DESC";

            object result = DBHelper.ExecuteScalar(query, CommandType.Text);

            if (result == null || result == DBNull.Value)
            {
                return "Optimum-0001";
            }

            string lastId = result.ToString();
            string numberPart = lastId.Substring("Optimum-".Length);

            if (int.TryParse(numberPart, out int lastNumber))
            {
                int nextNumber = lastNumber + 1;
                return $"Optimum-{nextNumber.ToString("D4")}";
            }

            return "Optimum-0001";
        }

        public List<int> GetSubOrdinatesByManager(int managerID)
        {
            var subordinates = new List<int>();

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ManagerID", managerID)
            };
            var res = DBHelper.ExecuteReader("usp_getSubOrdinatesByManager", CommandType.StoredProcedure, parameters);

            foreach (var row in res)
            {
                if (row.TryGetValue("EMP_ID", out object id) && id != null)
                {
                    subordinates.Add(Convert.ToInt32(id));
                }
            }
            return subordinates;
        }
}
}


