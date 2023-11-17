using System;
using System.ComponentModel.DataAnnotations;
namespace Ndknitor.Web.Validations;
[AttributeUsage(AttributeTargets.Property)]
public class PastDateAttribute : ValidationAttribute
{
    public bool CanEquals { get; set; } = false;



        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }
        if (value is DateTime dateTimeValue)
        {
            if (CanEquals)
            {
                if (dateTimeValue.Date > DateTime.Now.Date)
                    return new ValidationResult($"{validationContext.DisplayName} must be before or equals to the current date.");
                return ValidationResult.Success;
            }
            else
            {
                if (dateTimeValue.Date >= DateTime.Now.Date)
                    return new ValidationResult($"{validationContext.DisplayName} must be before the current date.");

                return ValidationResult.Success;
            }
        }
        throw new InvalidDataException("PastDateTimeAttribute expect a DateTime");
    }
}
