

using System.ComponentModel.DataAnnotations;

namespace HRMSModels
{
    public class ForgotPasswordModel
    {
        public string FirstName { get; set; }

        public int Emp_ID {  get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }
     
        public string ConfirmPassword { get; set; }
    }
}

