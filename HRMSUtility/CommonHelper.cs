using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSUtility
{
    public static class CommonHelper
    {
        public static bool AreEqual<T>(T a, T b)
        {
            return EqualityComparer<T>.Default.Equals(a, b);
        }

        public static bool AreNotEqual<T>(T a, T b)
        {
            return !EqualityComparer<T>.Default.Equals(a, b);
        }
    }
}
