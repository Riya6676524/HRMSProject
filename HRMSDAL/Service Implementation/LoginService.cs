using HRMSModels;
using HRMSUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HRMSDAL.Service;
using HRMSDAL.Helper;

namespace HRMSDAL.Service_Implementation
{
    public class LoginService : ILoginService
    {
        public LoginModel GetUserByEmail(string email)
        {
            return ExceptionHandler.Handle(() =>
            {
                string procedureName = "sp_GetUserByEmail";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = email }
                };

                var result = DBHelper.ExecuteReader(procedureName, CommandType.StoredProcedure, parameters);

                if (result.Count > 0)
                {
                    var row = result[0];

                    return new LoginModel
                    {
                        Emp_ID = Convert.ToInt32(row["Emp_ID"]),
                        Email = row["Email"]?.ToString(),
                        Password = row["Password"]?.ToString(),
                         RoleID = Convert.ToInt32(row["RoleID"])
                    };
                }

                return null;
            },
            defaultValue: null);
        }
    }
}


