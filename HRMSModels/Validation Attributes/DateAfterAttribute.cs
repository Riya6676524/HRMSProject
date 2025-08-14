using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HRMSModels
{
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public DateAfterAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
            ErrorMessage = "End Date must be after or equal to Start Date.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            if (startDateProperty == null)
                return new ValidationResult($"Unknown property: {_startDatePropertyName}");

            var startDateValue = startDateProperty.GetValue(validationContext.ObjectInstance);

            if (value is DateTime endDate && startDateValue is DateTime startDate)
            {
                if (endDate < startDate)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
