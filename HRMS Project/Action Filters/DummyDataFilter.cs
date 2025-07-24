using System;
using System.Web;
using System.Web.Mvc;

public class DummyDataFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        HttpSessionStateBase session = filterContext.HttpContext.Session;

        if (session["Emp_ID"] == null)
        {
            session["Emp_ID"] = 7;
        }

        if (session["RoleID"] == null)
        {
            session["RoleID"] = 1;
        }

        base.OnActionExecuting(filterContext);
    }
}
