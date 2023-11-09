using Ndknitor.Web.Validations;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class FutureDateTimeAttributeTests
{
    [Test]
    public void IsValid_WithFutureDateTimeAndCanEqualsFalse_ShouldReturnTrue()
    {
        // Arrange
        var attribute = new FutureDateTimeAttribute();
        var futureDateTime = DateTime.Now.AddMinutes(30);

        // Act
        var result = attribute.IsValid(futureDateTime);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsValid_WithCurrentDateTimeAndCanEqualsFalse_ShouldReturnFalse()
    {
        // Arrange
        var attribute = new FutureDateTimeAttribute();
        var currentDateTime = DateTime.Now;

        // Act
        var result = attribute.IsValid(currentDateTime);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsValid_WithPastDateTimeAndCanEqualsFalse_ShouldReturnFalse()
    {
        // Arrange
        var attribute = new FutureDateTimeAttribute();
        var pastDateTime = DateTime.Now.AddMinutes(-30);

        // Act
        var result = attribute.IsValid(pastDateTime);

        // Assert
        Assert.IsFalse(result);
    }
}
