using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Property)]
public class RegularTextAttribute : ValidationAttribute
{
    public bool IncludeSpace { get; set; } = true;
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
            string space = IncludeSpace ? " " : "";
            // Fix the regular expression pattern here
            Regex regex = new Regex($"^[A-Za-z{number}{Regex.Escape(IncludeCharaters)}{space}]+$");
            
            if (regex.IsMatch(target))
            {
                return true;
            }
        }
        else
        {
            throw new InvalidDataException("RegularText expects a string");
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} was not in the correct format";
    }
}
