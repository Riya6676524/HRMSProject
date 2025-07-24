using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS_Project.Models
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge = 0;
        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge -= minimumAge;
            ErrorMessage = $"You must be at least {_minimumAge} years old.";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            DateTime dob;
            if (DateTime.TryParse(value.ToString(), out dob))
            {
                return dob <= DateTime.Today.AddYears(_minimumAge);
            }

            return false;
        }
    }
}