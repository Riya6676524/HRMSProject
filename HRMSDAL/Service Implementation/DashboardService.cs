using HRMSDAL.Helper;
using HRMSDAL.Service;
using HRMSModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HRMSDAL.Service_Implementation
{
    public class DashboardService : IDashboardService
    {
        public NavbarModel GetNavbarData(int empId, int roleId)
        {
            NavbarModel model = new NavbarModel
            {
                MenuList = new List<MenuModel>()
            };

            var employeeData = DBHelper.ExecuteReader(
                "usp_GetNavbarInfoByEmpID",
                CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@Emp_ID", empId) }
            );

            if (employeeData.Count > 0)
            {
                var row = employeeData[0];

                model.EmployeeID = row["EmployeeID"]?.ToString();
                model.FirstName = row["FirstName"]?.ToString();
                model.MiddleName = row["MiddleName"]?.ToString();
                model.LastName = row["LastName"]?.ToString();
                model.ProfileImagePath = string.IsNullOrEmpty(row["ProfileImagePath"]?.ToString())
                    ? "/Content/Images/default.png"
                    : row["ProfileImagePath"]?.ToString();
                model.DepartmentName = row["DepartmentName"]?.ToString();
            }

            // Fetch menus via GenericService
            var menuService = new MenuService();
            var roleMenuService = new RoleMenuService();

            var allMenus = menuService.GetAll().OrderBy(m => m.DisplayOrder).ToList();
            var roleMenus = roleMenuService.GetAll().Where(rm => rm.RoleID == roleId).ToList();

            var filteredMenus = allMenus
                .Where(m => roleMenus.Any(rm => rm.MenuID == m.MenuID))
                .ToList();

            var parentMenus = filteredMenus
                .Where(m => m.ParentMenuID == null)
                .ToList();

            foreach (var parent in parentMenus)
            {
                parent.SubMenus = filteredMenus
                    .Where(m => m.ParentMenuID == parent.MenuID)
                    .ToList();

                model.MenuList.Add(parent);
            }

            return model;
        }
    }
}









