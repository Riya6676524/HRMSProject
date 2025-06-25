using HRMSDAL.Service;
using HRMSModels;
using HRMSUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HRMSDAL.Helper;

namespace HRMSDAL.Service_Implementation
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        
        public ForgotPasswordModel GetUserByEmail(string email)
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

                    return new ForgotPasswordModel
                    {
                        Email = row["Email"]?.ToString(),
                        Emp_ID = Convert.ToInt32(row["Emp_ID"]),
                        FirstName = row["FirstName"]?.ToString()

                    };
                }

                return null;
            }, defaultValue: null);
        }

     
        public void SaveResetToken(int empid, string email, string token)
        {
            ExceptionHandler.Handle(() =>
            {
                string procedureName = "sp_SavePasswordResetToken";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Emp_ID", SqlDbType.Int) { Value = empid },
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = email },
                    new SqlParameter("@Token", SqlDbType.VarChar) { Value = token },
                    new SqlParameter("@ExpiryDate", SqlDbType.DateTime) { Value = DateTime.Now.AddMinutes(30) }
                };

                DBHelper.ExecuteNonQuery(procedureName, CommandType.StoredProcedure, parameters);
            });
        }

        public bool IsValidToken(string email, string token)
        {
            return ExceptionHandler.Handle(() =>
            {
                string procedureName = "sp_IsValidPasswordResetToken";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = email },
                    new SqlParameter("@Token", SqlDbType.VarChar) { Value = token }
                };

                int count = Convert.ToInt32(DBHelper.ExecuteScalar(procedureName, CommandType.StoredProcedure, parameters));
                return count > 0;
            }, defaultValue: false);
        }

        public void UpdatePassword(string email, string newPassword, string token)
        {
            ExceptionHandler.Handle(() =>
            {
                string procedure = "sp_UpdatePassword";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = email },
                    new SqlParameter("@Password", SqlDbType.VarChar) { Value = newPassword },
                      new SqlParameter("@Token", SqlDbType.VarChar) { Value = token }
                };

                DBHelper.ExecuteNonQuery(procedure, CommandType.StoredProcedure, parameters);
            });
        }
    }
}
