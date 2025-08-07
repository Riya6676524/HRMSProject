using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HRMSDAL.Service;
using Quartz;

namespace HRMS_Project.Jobs
{
    public class CreditMonthlyLeaveJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var leaveBalanceService = DependencyResolver.Current.GetService<ILeaveBalanceService>();

            DateTime executionTime = context.FireTimeUtc.LocalDateTime;

            leaveBalanceService.CreditMonthlyLeaves();

            // Log the execution time or any other details if needed

            return Task.CompletedTask;
        }
    }
}