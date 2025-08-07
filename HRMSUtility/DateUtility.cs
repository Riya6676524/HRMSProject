using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HRMSUtility
{
    public class DateUtility
    {
        private DateTime from;
        private DateTime to;
        private bool fromSecondHalf = false;
        private bool uptoFirstHalf = false;
        private List<DateTime> holidays ;

        public DateUtility setFrom(DateTime from)
        {
            this.from = from;
            return this;
        }

        public DateUtility setTo(DateTime to)
        {
            this.to = to;
            return this;
        }

        public DateUtility setFromSecondHalf(bool fromSecondHalf)
        {
            this.fromSecondHalf = fromSecondHalf;
            return this;
        }

        public DateUtility setUptoFirstHalf(bool uptoFirstHalf)
        {
            this.uptoFirstHalf = uptoFirstHalf;
            return this;
        }

        public float CalculateTotalLeaveDays()
        {
            if (from == default || to == default)
                throw new InvalidOperationException("Both From and To dates must be set.");

            if (from > to)
                throw new InvalidOperationException("From date cannot be after To date.");

            float totalDays = 0;

            for (DateTime date = from; date <= to; date = date.AddDays(1))
            {
                if (!(holidays is null) && holidays.Contains(date.Date))
                    continue;

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;

                if (date == from)
                {
                    if (fromSecondHalf)
                        totalDays = 0.5f;
                    else
                        totalDays += 1;
                }
                else if (date == to)
                {
                    if (uptoFirstHalf)
                        totalDays += 0.5f;
                    else
                        totalDays += 1;
                }
                else
                {
                    totalDays += 1;
                }
            }

            return totalDays;
        }
    }
}
