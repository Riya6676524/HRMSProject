using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSModels;
using HRMSDAL.Service;
using HRMSDAL.Helper;
using System.Data;

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

    }
}
