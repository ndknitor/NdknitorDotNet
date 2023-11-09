using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace Ndknitor.Web.Validations;
[AttributeUsage(AttributeTargets.Property)]
public class RegularTextAttribute : ValidationAttribute
{
    public bool IncludeNumber { get; set; } = false;
    public string IncludeCharaters { get; set; } = "";
    public override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }
        if (value is string target)
        {
            string number = IncludeNumber ? "0-9" : "";
            Regex regex = new Regex($"[^a-zA-Z${number} ${IncludeCharaters}]+");
            if (regex.IsMatch(target))
            {
                return ValidationResult.Success;
            }
        }
        else
        {
            throw new Exception("RegularText expect a string");
        }

        return new ValidationResult($"{nameof(value)} was not in the correct format");
    }
}
