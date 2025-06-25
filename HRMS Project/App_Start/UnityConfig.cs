using HRMSDAL.Service;
using HRMSDAL.Service_Implementation;
using HRMSDAL.ServiceImplementation;
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
            // Use the same container (don't create a new one)
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            // Register your services
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IDashboardService, DashboardService>();



        }
    }
}
