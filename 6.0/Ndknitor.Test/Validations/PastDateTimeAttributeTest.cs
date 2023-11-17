using Ndknitor.Web.Validations;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class PastDateTimeAttributeTest
{

    [Test]
    public void IsValid_WithPastDateTimeAndCanEqualsFalse_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute();
        var pastDateTime = DateTime.Now.AddMinutes(-5); // 5 minutes ago

        // Act
        var result = attribute.GetValidationResult(pastDateTime, new ValidationContext(DateTime.Now));

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public void IsValid_WithFutureDateTimeAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute();
        var futureDateTime = DateTime.Now.AddMinutes(5); // 5 minutes from now

        // Act
        var result = attribute.GetValidationResult(futureDateTime, new ValidationContext(DateTime.Now));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("DateTime must be before the current date and time."));
    }

    [Test]
    public void IsValid_WithNonDateTimeValue_ShouldThrow()
    {
        // Arrange
        var attribute = new PastDateTimeAttribute();
        var nonDateTimeValue = "NotADateTime";

        // Act
        Assert.Throws<InvalidDataException>(() =>
        {
            attribute.GetValidationResult(nonDateTimeValue, new ValidationContext(1));
        });
    }
}
