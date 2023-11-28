using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Ndknitor.Web.Validations;
public class JwtTokenAttribute : ValidationAttribute
{
    private readonly string _jwtPattern = @"^eyJ[a-zA-Z0-9_-]*\.[a-zA-Z0-9_-]*\.[a-zA-Z0-9_-]*$";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is string jwt)
        {
            if (jwt == "")
            {
                return ValidationResult.Success;
            }
            if (Regex.IsMatch(jwt, _jwtPattern))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid JWT format.");
            }
        }

        return new ValidationResult("Invalid JWT format.");
    }
}