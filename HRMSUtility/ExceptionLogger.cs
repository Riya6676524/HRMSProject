using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;

namespace HRMSUtility
{
    public static class ExceptionLogger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExceptionLogger));

        public static void LogException(Exception ex,int empId= 0)
        {
            if (empId == 0 && HttpContext.Current?.Session?["Emp_ID"] != null)
            {
                int.TryParse(HttpContext.Current.Session["Emp_ID"].ToString(), out empId);
            }
            StackTrace st = new StackTrace(ex, true);
            StackFrame frame = st.GetFrame(0);

            int line = frame?.GetFileLineNumber() ?? 0;
            string methodName = frame?.GetMethod()?.Name ?? "Unknown";
            string source = ex.Source ?? "Unknown";

            log4net.ThreadContext.Properties["Emp_ID"] = empId;
            log4net.ThreadContext.Properties["LineNumber"] = line;
            log4net.ThreadContext.Properties["MethodName"] = methodName;
            log4net.ThreadContext.Properties["Source"] = source;

            log.Error(ex.Message, ex);
        }
    }
}
