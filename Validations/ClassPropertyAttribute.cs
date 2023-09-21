using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Ndknitor.Web.Validations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ClassPropertyAttribute : ValidationAttribute
{
    private readonly Type _targetType;

    public ClassPropertyAttribute(Type targetType)
    {
        _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        IEnumerable<string> propertyNames = null;

        if (value is string singlePropertyName)
        {
            propertyNames = new string[] { singlePropertyName };
        }
        else if (value is IEnumerable<string> arrayPropertyNames)
        {
            propertyNames = arrayPropertyNames;
        }
        else
        {
            return new ValidationResult("Invalid property names.");
        }

        PropertyInfo[] properties = _targetType.GetProperties();

        foreach (var propertyName in propertyNames)
        {

            bool isPropertyValid = false;

            foreach (PropertyInfo property in properties)
            {
                if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    isPropertyValid = true;
                    break;
                }
            }

            if (!isPropertyValid)
            {
                return new ValidationResult($"{propertyName} is not a valid property of {_targetType.Name}.");
            }
        }

        return ValidationResult.Success;
    }
}