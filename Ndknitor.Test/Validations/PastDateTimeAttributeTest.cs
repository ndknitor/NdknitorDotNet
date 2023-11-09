using Ndknitor.Web.Validations;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class PastDateTimeAttributeTest
{
    [Test]
    public void IsValid_WithPastDateTimeAndCanEqualsTrue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute { CanEquals = true };
        var pastDateTime = DateTime.Now.AddMinutes(-5); // 5 minutes ago

        // Act
        var result = attribute.GetValidationResult(pastDateTime, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithPastDateTimeAndCanEqualsFalse_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute();
        var pastDateTime = DateTime.Now.AddMinutes(-5); // 5 minutes ago

        // Act
        var result = attribute.GetValidationResult(pastDateTime, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithCurrentDateTimeAndCanEqualsTrue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute { CanEquals = true };
        var currentDateTime = DateTime.Now;

        // Act
        var result = attribute.GetValidationResult(currentDateTime, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithCurrentDateTimeAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute();
        var currentDateTime = DateTime.Now;

        // Act
        var result = attribute.GetValidationResult(currentDateTime, new ValidationContext(null));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before the current date and time."));
    }

    [Test]
    public void IsValid_WithFutureDateTimeAndCanEqualsTrue_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute { CanEquals = true };
        var futureDateTime = DateTime.Now.AddMinutes(5); // 5 minutes from now

        // Act
        var result = attribute.GetValidationResult(futureDateTime, new ValidationContext(null));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before or equals to the current date and time."));
    }

    [Test]
    public void IsValid_WithFutureDateTimeAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute();
        var futureDateTime = DateTime.Now.AddMinutes(5); // 5 minutes from now

        // Act
        var result = attribute.GetValidationResult(futureDateTime, new ValidationContext(null));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before the current date and time."));
    }

    [Test]
    public void IsValid_WithNonDateTimeValue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute();
        var nonDateTimeValue = "NotADateTime";

        // Act
        var result = attribute.GetValidationResult(nonDateTimeValue, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }
}
