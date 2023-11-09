using Ndknitor.Web.Validations;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class PastDateAttributeTests
{
    [Test]
    public void IsValid_WithPastDateAndCanEqualsTrue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateAttribute { CanEquals = true };
        var pastDate = DateTime.Now.AddDays(-1);

        // Act
        var result = attribute.GetValidationResult(pastDate, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithPastDateAndCanEqualsFalse_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateAttribute();
        var pastDate = DateTime.Now.AddDays(-1);

        // Act
        var result = attribute.GetValidationResult(pastDate, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithCurrentDateAndCanEqualsTrue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateAttribute { CanEquals = true };
        var currentDate = DateTime.Now;

        // Act
        var result = attribute.GetValidationResult(currentDate, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithCurrentDateAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateAttribute();
        var currentDate = DateTime.Now;

        // Act
        var result = attribute.GetValidationResult(currentDate, new ValidationContext(null));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before the current date and time."));
    }

    [Test]
    public void IsValid_WithFutureDateAndCanEqualsTrue_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateAttribute { CanEquals = true };
        var futureDate = DateTime.Now.AddDays(1);

        // Act
        var result = attribute.GetValidationResult(futureDate, new ValidationContext(null));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before or equals to the current date and time."));
    }

    [Test]
    public void IsValid_WithFutureDateAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateAttribute();
        var futureDate = DateTime.Now.AddDays(1);

        // Act
        var result = attribute.GetValidationResult(futureDate, new ValidationContext(null));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before the current date and time."));
    }

    [Test]
    public void IsValid_WithNonDateValue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateAttribute();
        var nonDateValue = "NotADate";

        // Act
        var result = attribute.GetValidationResult(nonDateValue, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }
}
