using System;
using System.ComponentModel.DataAnnotations;
namespace Ndknitor.Web.Validations;
[AttributeUsage(AttributeTargets.Property)]
public class FutureDateTimeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }
        if (value is DateTime dateTimeValue)
        {
            if (dateTimeValue <= DateTime.Now)
            {
                return new ValidationResult("");
            }
            return ValidationResult.Success;
        }
        throw new InvalidDataException("FutureDateTime expect a DateTime");
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be after the current date and time.";
    }
}
