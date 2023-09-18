This class library mainly write for request validation in ASP.NET Core and some extension method for DbContext in EF Core

# Ndknitor.Web.Validations Namespace

The `Ndknitor.Web.Validations` namespace contains custom validation attributes for use in ASP.NET applications.

## `AllowedExtensionsAttribute` Class

### Description
The `AllowedExtensionsAttribute` is a custom validation attribute that allows you to specify a list of allowed file extensions for file uploads using `IFormFile` in ASP.NET applications.

### Constructor
```csharp
public AllowedExtensionsAttribute(params string[] allowedExtensions)

## `ClassPropertyAttribute` Class

### Description
The `ClassPropertyAttribute` is a custom validation attribute used for ensuring that a property name is valid for a specified target type. It is particularly useful for validating input data where property names need to be validated against a specific class type.

### Constructor
```csharp
public ClassPropertyAttribute(Type targetType)

[ClassProperty(typeof(Person))] // Validates that property names are valid for the Person class.
public IEnumerable<string> PropertyNames { get; set; }

## `FutureDateAttribute` Class

### Description
The `FutureDateAttribute` is a custom validation attribute used to ensure that a `DateTime` value is set to a date and time in the future, optionally including the current date and time, depending on the configuration. It is designed for use in ASP.NET applications to validate date inputs.

### Properties

#### `CanEquals` Property
```csharp
public bool CanEquals { get; set; } = false;

[FutureDate(CanEquals = false)] // Validates that the date is in the future (not including the current date and time).
public DateTime EventDate { get; set; }
