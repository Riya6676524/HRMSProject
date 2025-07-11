using HRMSModels;
using HRMSDAL;
using HRMSUtility;
using System;
using System.Web.Mvc;
using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using System.Linq;
using System.Reflection;

namespace HRMSProject.Controllers
{
    public class ForgotPasswordController : Controller
    {

        private readonly IForgotPasswordService _forgotpasswordservice;

        public ForgotPasswordController(IForgotPasswordService forgotpasswordservice)
        {
            _forgotpasswordservice = forgotpasswordservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ForgotPasswordModel model)
        {
            return ExceptionHandler.Handle(() =>
            {

                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    ModelState.AddModelError("Email", "Please enter email");
                    return View(model);
                }

                var user = _forgotpasswordservice.GetUserByEmail(model.Email);
                if (CommonHelper.AreEqual(user, null))
                {
                    ModelState.AddModelError("Email", "Invalid Credential");
                    return View(model);
                }
                else if (user != null)
                {
                    string token = Guid.NewGuid().ToString();
                    string link = Url.Action("ResetPassword", "ForgotPassword", new { email = model.Email, token = token }, protocol: Request.Url.Scheme);

                    _forgotpasswordservice.SaveResetToken(user.Emp_ID, model.Email, token);

                    string subject = "Reset your HRMS password";
                    string body = $@"
                    <p>Dear {user.FirstName},</p>
                    <p>We received a request to reset your password</p>
                    <p> Click the following link to reset your password:</p><br/>
                    <a href='{link}'>Reset Password</a><br/>
                    <br/>
                    <p>Regards,<br/>HRMS Team</p>";


                    bool mailSent = EmailHelper.SendEmail(model.Email, subject, body);

                    if (mailSent)
                    {
                        TempData["Message"] = "Link Send Sucessfully";

                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to send email. Try again.");
                    }
                }
                return View(model);
            }, defaultValue: View(model));
        }



        [HttpGet]
        public ActionResult ResetPassword(string email, string token)
        {
            if (!_forgotpasswordservice.IsValidToken(email, token))
            {
                TempData["Error"] = "Invalid or expired password reset link.";
            }

            var model = new ForgotPasswordModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ResetPassword(ForgotPasswordModel model)
        {
            return ExceptionHandler.Handle(() =>
            {

                if (string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    ModelState.AddModelError("NewPassword", "New password is required.");
                    return View(model);
                }
                if (!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    string password = model.NewPassword;

                    if (password.Length < 8)
                    {
                        ModelState.AddModelError("NewPassword", "Password must be at least 8 characters long");
                        return View(model);
                    }

                    if (!password.Any(char.IsUpper))
                    {
                        ModelState.AddModelError("NewPassword", "Password must contain at least one uppercase letter [A-Z]");
                        return View(model);
                    }
                    if (!password.Any(char.IsLower))
                    {
                        ModelState.AddModelError("NewPassword", "Password must contain at least one lowercase letter [a-z]");
                        return View(model);
                    }
                    if (!password.Any(char.IsDigit))
                    {
                        ModelState.AddModelError("NewPassword", "Password must contain at least one digit [0-9]");
                        return View(model);
                    }
                    if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                    {
                        ModelState.AddModelError("NewPassword", "Password must contain at least one special character (@, #, $, &, *, !)");
                        return View(model);
                    }
                }


                if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "Confirm password is required.");
                    return View(model);
                }

                if (!string.IsNullOrWhiteSpace(model.NewPassword) && !string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    if (model.NewPassword != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Passwords do not match");
                        return View(model);
                    }
                }

                if (!_forgotpasswordservice.IsValidToken(model.Email, model.Token))
                {
                    ModelState.AddModelError("", "Token is invalid or expired.");
                    return View(model);
                }


                string encodedPassword = Base64Helper.Encode(model.NewPassword);
                _forgotpasswordservice.UpdatePassword(model.Email, encodedPassword, model.Token);

                TempData["Message"] = "Password updated successfully.";
                return View(model);

            }, defaultValue: View(model));
        }
    }

}