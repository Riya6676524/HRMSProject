using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HRMSExceptionHandling
{
    public static class ExceptionLogger
    {
    
        private static readonly string logFilePath = @"C:\Logs\HRMS_ErrorLog.txt";

        public static void LogException(Exception ex)
        {
            try
            {
              
                string folderPath = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

              
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("========== EXCEPTION ==========");
                    writer.WriteLine($"Time          : {DateTime.Now}");
                    writer.WriteLine($"Message       : {ex.Message}");
                    writer.WriteLine($"Stack Trace   : {ex.StackTrace}");
                    if (ex.InnerException != null)
                        writer.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    writer.WriteLine("===============================");
                    writer.WriteLine();
                }
            }
            catch
            {
             //By using a silent catch
             //you protect your application from secondary failures due to logging
             //OR you can add the Fallback Logger: A fallback logger is a backup mechanism
             //used to log errors when your primary logging method fails
            }
        }
    }
}
