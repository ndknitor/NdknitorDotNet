This class library mainly write for request validation in ASP.NET Core and some extension method for DbContext in EF Core

# Ndknitor.Web.Validations Namespace

- **AllowedExtensionsAttribute**: Validates file extensions for `IFormFile` uploads.
- **ClassPropertyAttribute**: Validates whether property names are valid for a specified target type.
- **FutureDateAttribute**: Ensures that a `DateTime` value is set to a date and time in the future.
- **FutureDateTimeAttribute**: Ensures that a `DateTime` value is set to a date and time in the future, including the current date and time.
- **GreaterThan**: Validates that a property's value is greater than another property's value.
- **ImageFileAttribute**: Validates that an uploaded file is an image based on its content type.
- **LeastOnePropertyAttribute**: Ensures that at least one of the specified properties has a non-null or non-empty value.
- **LessThan**: Validates that a property's value is less than another property's value.
- **MaxFileSizeAttribute**: Validates the maximum file size for `IFormFile` uploads.
- **OnlyOnePropertyAttribute**: Ensures that only one of the specified properties has a value.
- **PastDateAttribute**: Ensures that a `DateTime` value is set to a date and time in the past.
- **PastDateTimeAttribute**: Ensures that a `DateTime` value is set to a date and time in the past, including the current date and time.
- **SquareImageAttribute**: Validates that an uploaded image has a square aspect ratio.
