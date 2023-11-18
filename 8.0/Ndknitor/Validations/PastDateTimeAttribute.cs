using System;
using System.ComponentModel.DataAnnotations;
namespace Ndknitor.Web.Validations;
[AttributeUsage(AttributeTargets.Property)]
public class PastDateTimeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }
        if (value is DateTime dateTimeValue)
        {
            if (dateTimeValue >= DateTime.Now)
                return new ValidationResult($"{validationContext.DisplayName} must be before the current date and time.");

            return ValidationResult.Success;
        }
        throw new InvalidDataException("PastDateTimeAttribute expect a DateTime");
    }
}
