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
using HRMSModels;

namespace HRMSDAL.Service_Implementation
{
public class EmployeeService : GenericService<EmployeeModel>, IEmployeeService
    {
        protected override string TableName => "Employee";
        protected override string PrimaryKeyColumn => "EMP_ID";
        
        
    public EmployeeModel GetProfile(int empId)
    {
        EmployeeModel model = new EmployeeModel();


        var result = DBHelper.ExecuteReader(
                "usp_GetMyProfileData",
                CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@Emp_ID", empId) }
            );

        if (result.Count > 0)
        {
            var row = result[0];

            model.FirstName = row["FirstName"]?.ToString();
            model.MiddleName = row["MiddleName"]?.ToString();
            model.LastName = row["LastName"]?.ToString();
            model.Email = row["Email"]?.ToString();
            model.ContactNumber = Convert.ToInt64(row["ContactNumber"]);
            model.ProfileImagePath = row["ProfileImagePath"] as byte[];
            model.Password = row["Password"]?.ToString();
            model.Address = row["Address"]?.ToString();
            model.StateName = row["StateName"]?.ToString();
            model.CityName = row["CityName"]?.ToString();
            model.CountryName = row["CountryName"]?.ToString();
            model.RoleName = row["RoleName"]?.ToString();
        }
        return model;

    }


 
      public bool ChangePassword(int empId, string currentPassword, string newPassword)
    {
     
        var param = new SqlParameter[] {
                new SqlParameter("@Emp_ID", empId),
                new SqlParameter("@CurrentPassword", currentPassword),
                new SqlParameter("@NewPassword", newPassword)
            };

        int result = DBHelper.ExecuteNonQuery("sp_ChangePassword", CommandType.StoredProcedure, param);
        return result > 0;
    }
    
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
}

