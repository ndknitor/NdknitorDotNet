using Ndknitor.Web.Validations;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class RegularTextAttributeTests
{
    [Test]
    public void IsValid_WithValidString_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new RegularTextAttribute { IncludeNumber = true, IncludeCharaters = "!@#" };
        var validString = "abc123!@#";

        // Act
        var result = attribute.GetValidationResult(validString, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithValidStringAndNoNumbers_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new RegularTextAttribute { IncludeNumber = false, IncludeCharaters = "!@#" };
        var validString = "abc!@#";

        // Act
        var result = attribute.GetValidationResult(validString, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithValidStringAndNoSpecialCharacters_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new RegularTextAttribute { IncludeNumber = true, IncludeCharaters = "" };
        var validString = "abc123";

        // Act
        var result = attribute.GetValidationResult(validString, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithValidStringAndNoNumbersOrSpecialCharacters_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new RegularTextAttribute { IncludeNumber = false, IncludeCharaters = "" };
        var validString = "abc";

        // Act
        var result = attribute.GetValidationResult(validString, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithInvalidString_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new RegularTextAttribute { IncludeNumber = true, IncludeCharaters = "!@#" };
        var invalidString = "abc123$%^";

        // Act
        var result = attribute.GetValidationResult(invalidString, new ValidationContext(null));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("Value was not in the correct format"));
    }

    [Test]
    public void IsValid_WithNullValue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new RegularTextAttribute();

        // Act
        var result = attribute.GetValidationResult(null, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithNonStringValue_ShouldThrowException()
    {
        // Arrange
        var attribute = new RegularTextAttribute();
        var nonStringValue = 123;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => attribute.GetValidationResult(nonStringValue, new ValidationContext(null)));
    }
}
