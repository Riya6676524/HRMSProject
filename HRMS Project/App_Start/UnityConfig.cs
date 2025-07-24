using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using System;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace HRMS_Project
{
    public static class UnityConfig
    {
        #region Unity Container

        private static readonly Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var unityContainer = new UnityContainer();
            RegisterTypes(unityContainer);
            return unityContainer;
        });

        public static IUnityContainer Container => container.Value;

        #endregion

        public static void RegisterComponents()
        {

            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {

            container.RegisterType<ILoginService, LoginService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IDashboardService, DashboardService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IRoleMenuService, RoleMenuService>();
            container.RegisterType<IEmployeeService, EmployeeService>();
            container.RegisterType<ILeaveRequestService, LeaveRequestService>();
            container.RegisterType<IGenderService, GenderService>();
            container.RegisterType<ICountryService, CountryService>();
            container.RegisterType<IStateService, StateService>();
            container.RegisterType<ICityService, CityService>();
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IRoleMenuService, RoleMenuService>();
            container.RegisterType<ILeaveTypeService, LeaveTypeService>();
            container.RegisterType<ILeaveStatusService, LeaveStatusService>();
            container.RegisterType<IAttendanceService, AttendanceService>();
            container.RegisterType<IHolidayService, HolidayService>();



        }
    }
}
