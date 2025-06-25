using HRMSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSDAL.Service
{
    public interface IForgotPasswordService
    {
         ForgotPasswordModel GetUserByEmail(string email);
        void SaveResetToken(int empid,string email, string token);
        bool IsValidToken(string email, string token);
        void UpdatePassword(string email, string newPassword, string token);
    }
    }
