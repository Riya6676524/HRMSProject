﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSUtility
{
    public class ExceptionHandler
    {
        public static T Handle<T>(Func<T> func, T defaultValue = default)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return defaultValue;
            }
        }

        public static void Handle(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }
    }
}
