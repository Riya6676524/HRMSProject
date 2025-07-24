using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Web.UI;
using HRMSDAL.Service;
using HRMSModels;
using HRMSProject.Models;
using HRMSUtility;

namespace HRMS.Controllers
{
    //[MenuFilter]
    [DummyDataFilter]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IGenderService _genderService;
        private readonly IDepartmentService _departmentService;
        private readonly IRoleService _roleService;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;

        public EmployeeController(
            IEmployeeService employeeService,
            IGenderService genderService,
            IDepartmentService departmentService,
            IRoleService roleService,
            ICountryService countryService,
            IStateService stateService,
            ICityService cityService
            )
        {
            _employeeService = employeeService;
            _genderService = genderService;
            _departmentService = departmentService;
            _roleService = roleService;
            _countryService = countryService;
            _stateService = stateService;
            _cityService = cityService;

        }


        public ActionResult EmployeeGridPartial()
        {
            var EmployeeList = _employeeService.GetAll();
            var EmployeeViewList = new List<EmployeeListModel>();
            foreach (EmployeeModel item in EmployeeList)
            {
                EmployeeViewList.Add(new EmployeeListModel(item));
            }
            return View(EmployeeViewList);
        }

        private void PopulateDropdowns()
        {
            var countries = _countryService.GetAll() ?? new List<CountryModel>();
            var genders = _genderService.GetAll() ?? new List<GenderModel>();
            var departments = _departmentService.GetAll() ?? new List<DepartmentModel>();
            var roles = _roleService.GetAll() ?? new List<RoleModel>();
            var managers = _employeeService.GetAll() ?? new List<EmployeeModel>();

            ViewBag.Countries = new SelectList(countries, "CountryID", "CountryName");
            ViewBag.Genders = new SelectList(genders, "GenderID", "GenderName");
            ViewBag.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
            ViewBag.Roles = new SelectList(roles, "RoleID", "RoleName");
            ViewBag.ReportingManagers = new SelectList(managers, "EMP_ID", "FirstName");
        }

        public ActionResult Employees()
        {
            var EmployeeList = _employeeService.GetAll();
            var EmployeeViewList = new List<EmployeeListModel>();
            foreach (EmployeeModel item in EmployeeList)
            {
                EmployeeViewList.Add(new EmployeeListModel(item));
            }
            return View(EmployeeViewList);
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

            regModel.ModifiedOn = DateTime.Now;
            regModel.CreatedOn = DateTime.Now;
            regModel.Password = Base64Helper.Encode(regModel.Password);
            _employeeService.Insert(regModel);

            ViewBag.message = new MesssageBoxViewModel()
            {
                head = "Employee Registered Successfully",
                body = $"<p><strong>Employee ID:</strong> {regModel.EmployeeID}</p>" +
                       $"<p><strong>Email:</strong> {regModel.Email}</p>" +
                       $"<p><strong>Password:</strong> {regModel.Password}</p>"
            };
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

        public ActionResult DeleteEmployee(EmployeeModel obj)
        {
            var employee = _employeeService.GetById(obj.EMP_ID);
            return View(employee);
        }

        [HttpPost]
        public JsonResult DeleteEmployeeConfirmed(int empId)
        {
            _employeeService.Delete(empId);
            return Json(new { success = true });
        }
    }
}