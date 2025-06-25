using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSDAL;
using HRMSModels;
using HRMSUtility;
using HRMSExceptionHandling;

namespace HRMSService
{
    public class LoginService
    {
        private readonly LoginDAL dal = new LoginDAL();

        public string ValidateLogin(string Email, string Password)
        {
            return ExceptionHandler.Handle(() =>
            {
                LoginModel user = dal.GetUserByEmail(Email);

                if (UtilityHelper.AreEqual(user, null))
                    return "Invalid email";

                if (UtilityHelper.AreEqual(user.Password, null))
                    return "Invalid Password";

                string decodedPassword = Base64Helper.Decode(user.Password);

                if (string.IsNullOrWhiteSpace(decodedPassword))
                    return "Stored password is invalid";

                if (UtilityHelper.AreNotEqual(decodedPassword, Password))
                    return "Invalid password";

                return "Success";

            }, "Login Failed!");
        }
    }

}


