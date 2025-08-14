using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Web.UI;
using HRMSProject.Models;
using HRMSUtility;
using System.Reflection;

namespace HRMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeProfileService _employeeProfileService;
        private readonly IGenderService _genderService;
        private readonly IDepartmentService _departmentService;
        private readonly IRoleService _roleService;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly ILocationService _locationService;

        public EmployeeController(
            IEmployeeService employeeService,
            IEmployeeProfileService employeeProfileService,
            IGenderService genderService,
            IDepartmentService departmentService,
            IRoleService roleService,
            ICountryService countryService,
            IStateService stateService,
            ICityService cityService,
            ILocationService locationService
            )
        {
            _employeeService = employeeService;
            _employeeProfileService = employeeProfileService;
            _genderService = genderService;
            _departmentService = departmentService;
            _roleService = roleService;
            _countryService = countryService;
            _stateService = stateService;
            _cityService = cityService;
            _locationService = locationService;
        }


        public ActionResult EmployeeGridPartial()
        {

            var EmployeeViewList = _employeeService.GetAll();
            return View(EmployeeViewList);
        }


        private void PopulateDropdowns()
        {
            var countries = _countryService.GetAll() ?? new List<CountryModel>();
            var genders = _genderService.GetAll() ?? new List<GenderModel>();
            var departments = _departmentService.GetAll() ?? new List<DepartmentModel>();
            var roles = _roleService.GetAll() ?? new List<RoleModel>();
            var managers = _employeeService.GetAll().Where(x => x.RoleID == 2) ?? new List<EmployeeModel>();
            var locations = _locationService.GetAll();

            ViewBag.Countries = new SelectList(countries, "CountryID", "CountryName");
            ViewBag.Genders = new SelectList(genders, "GenderID", "GenderName");
            ViewBag.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
            ViewBag.Roles = new SelectList(roles, "RoleID", "RoleName");
            ViewBag.ReportingManagers = new SelectList(managers, "EMP_ID", "FirstName");
            ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");
        }

        public ActionResult Employees()
        {
            
            var EmployeeViewList = _employeeService.GetAll();
            return View(EmployeeViewList);
        }

        [HttpGet]
        public ActionResult Profile()
        {
            int empId = Convert.ToInt32(Session["Emp_ID"]);
            var model = _employeeProfileService.GetProfile(empId);
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        public ActionResult Add(EmployeeRegModel regModel)
        {

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View("Add", regModel);
            }
            regModel.CreatedByID = Convert.ToInt32(Session["Emp_ID"]);
            regModel.ModifiedByID = Convert.ToInt32(Session["Emp_ID"]);
            regModel.ModifiedOn = DateTime.Now;
            regModel.CreatedOn = DateTime.Now;
            regModel.Password = Base64Helper.Encode(regModel.Password);
            _employeeService.Insert(regModel);
            return RedirectToAction("Employees");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var employee = _employeeService.GetById(id);
            PopulateDropdowns();
            ViewBag.States = new SelectList(_stateService.GetAll().Where(s => s.CountryID == employee.CountryID).ToList(),"StateID","StateName");
            ViewBag.Cities = new SelectList(_cityService.GetAll().Where(s => s.StateID == employee.StateID).ToList(), "CityID", "CityName");
            return View(new EmployeeRegModel(employee));
        }

        [HttpPost]
        public ActionResult Edit(EmployeeRegModel regModel)
        {
            var employee = _employeeService.GetById(regModel.EMP_ID);
            if (regModel.Password is null)
            {
                ModelState.Remove("Password"); // Exclude Password from validation if not changing
                ModelState.Remove("ConfirmPassword"); // Exclude ConfirmPassword from validation if not changing
            }
            else
            {
                ViewBag.appendPasswordFields = true;
            }
            ModelState.Remove("ProfilePostedFile"); // Exclude ProfilePostedFile from validation if not changing
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                ViewBag.States = new SelectList(_stateService.GetAll().Where(s => s.CountryID == employee.CountryID).ToList(), "StateID", "StateName");
                ViewBag.Cities = new SelectList(_cityService.GetAll().Where(s => s.StateID == employee.StateID).ToList(), "CityID", "CityName");
                return View("Edit", regModel);
            }

            if (regModel.ProfileImagePath is null) regModel.ProfileImagePath = employee.ProfileImagePath;
            if (regModel.Password is null) regModel.Password = employee.Password;
            else
            {
                regModel.Password = Base64Helper.Encode(regModel.Password);
            }
            regModel.CreatedByID = employee.CreatedByID;
            regModel.ModifiedByID = Convert.ToInt32(Session["Emp_ID"]);
            regModel.ModifiedOn = DateTime.Now;
            regModel.CreatedOn = employee.CreatedOn;

            _employeeService.Update(regModel);
            return RedirectToAction("Employees");
        }

        public JsonResult GetLatestEmployeeId()
        {
            var latestId = _employeeService.GetNextAvailableEmployeeId();
            return Json(new { latestId = latestId }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetStates(int countryID)
        {
            var states = _stateService.GetAll().Where(s => s.CountryID == countryID)
                            .Select(s => new { s.StateID, s.StateName })
                            .ToList();

            return Json(states, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCities(int stateID)
        {
            var cities = _cityService.GetAll().Where(c => c.StateID == stateID)
                           .Select(c => new { c.CityID, c.CityName })
                           .ToList();

            return Json(cities, JsonRequestBehavior.AllowGet);
        }




        public ActionResult UpdateEmployee(EmployeeModel obj)
        {
            var employee = _employeeService.GetById(obj.EMP_ID);
            return View(employee);
        }

        [HttpPost]
        public JsonResult UpdateEmployeePost(EmployeeModel obj)
        {
            _employeeService.Update(obj);
            return Json(new { success = true });
        }

        public ActionResult Delete(int id)
        {
            _employeeService.Delete(id);
            return RedirectToAction("Employees");
        }

        public ActionResult DeletePromptPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ChangePassword(EmployeeProfileModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int empId = Convert.ToInt32(Session["Emp_ID"]);

            if (string.IsNullOrWhiteSpace(model.currpassword))
            {
                ModelState.AddModelError("currpassword", "Current password is required.");
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.newpassword))
            {
                ModelState.AddModelError("newpassword", "New password is required.");
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.confirmNewPassword))
            {
                ModelState.AddModelError("confirmNewPassword", "Confirm New password is required.");
                return View(model);
            }

            if (model.newpassword.Length < 8 ||
                !model.newpassword.Any(char.IsUpper) ||
                !model.newpassword.Any(char.IsLower) ||
                !model.newpassword.Any(char.IsDigit) ||
                !model.newpassword.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ModelState.AddModelError("newpassword", "Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.");
                return View(model);
            }

            if (model.newpassword != model.confirmNewPassword)
            {
                ModelState.AddModelError("confirmNewPassword", "Passwords do not match");
                return View(model);
            }

            string storedPassword = _employeeProfileService.GetEncodedPassword(empId);
            string enteredCurrentPassword = Base64Helper.Encode(model.currpassword);

            if (storedPassword != enteredCurrentPassword)
            {
                ModelState.AddModelError("currpassword", "Invalid current password");
                return View(model);
            }

            string encodedNewPassword = Base64Helper.Encode(model.newpassword);
            _employeeProfileService.ChangePassword(empId, encodedNewPassword);

                TempData["Message"] = "Password updated successfully!";
                return RedirectToAction("ChangePassword");
            }
        }


    }




