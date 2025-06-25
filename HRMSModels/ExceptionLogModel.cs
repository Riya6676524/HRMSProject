using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSModels
{
    public class ExceptionLogModel
    {
        public int Emp_ID { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public int LineNumber { get; set; }
        public string MethodName { get; set; }
        public string Source { get; set; }
        public string InnerException { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
