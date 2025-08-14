using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace HRMS_Project.App_Start
{
    public static class QuartzSchedular
    {

        private static IScheduler scheduler;
        public static void Start()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            
            IJobDetail job = JobBuilder.Create<Jobs.CreditMonthlyLeaveJob>()
                .WithIdentity("CreditMonthlyLeaveJob", "HRMS")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("CreditMonthlyLeaveTrigger", "HRMS")
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1,0,0).InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")))
                .Build();

            scheduler.ScheduleJob(job,trigger);
            scheduler.Start();
        }
    }
}