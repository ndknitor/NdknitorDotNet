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
        var result = attribute.GetValidationResult(pastDate, new ValidationContext(1));

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
        var result = attribute.GetValidationResult(pastDate, new ValidationContext(1));

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
        var result = attribute.GetValidationResult(currentDate, new ValidationContext(1));

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
        var result = attribute.GetValidationResult(currentDate, new ValidationContext(DateTime.Now));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before the current date."));
    }

    [Test]
    public void IsValid_WithFutureDateAndCanEqualsTrue_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateAttribute { CanEquals = true };
        var futureDate = DateTime.Now.AddDays(1);

        // Act
        var result = attribute.GetValidationResult(futureDate, new ValidationContext(DateTime.Now));
        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before or equals to the current date."));
    }

    [Test]
    public void IsValid_WithFutureDateAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateAttribute();
        var futureDate = DateTime.Now.AddDays(1);

        // Act
        var result = attribute.GetValidationResult(futureDate, new ValidationContext(DateTime.Now));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before the current date."));
    }

    [Test]
    public void IsValid_WithNonDateValue_ShouldReturnException()
    {
        // Arrange
        var attribute = new PastDateAttribute();
        var nonDateValue = "NotADate";

        // Assert
        Assert.Throws<InvalidDataException>(() =>
        {
            var result = attribute.GetValidationResult(nonDateValue, new ValidationContext(1));
        });
    }
}
