using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace Ndknitor.Web.Validations;
[AttributeUsage(AttributeTargets.Property)]
public class RegularTextAttribute : ValidationAttribute
{
    public bool IncludeNumber { get; set; } = false;
    public string IncludeCharaters { get; set; } = "";
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }
        if (value is string target)
        {
            string number = IncludeNumber ? "0-9" : "";
            Regex regex = new Regex($"[^a-zA-Z${number} ${IncludeCharaters}]+");
            if (regex.IsMatch(target))
            {
                return true;
            }
        }
        else
        {
            throw new Exception("RegularText expect a string");
        }

        return false;
    }
    public override string FormatErrorMessage(string name)
    {
        return $"{name} was not in the correct format";
    }
}
