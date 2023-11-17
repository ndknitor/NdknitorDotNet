using Ndknitor.Web.Validations;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class FutureDateAttributeTests
{
    [Test]
    public void IsValid_WithFutureDateAndCanEqualsTrue_ShouldReturnTrue()
    {
        // Arrange
        var attribute = new FutureDateAttribute { CanEquals = true };
        var futureDate = DateTime.Now.AddDays(1);

        // Act
        var result = attribute.IsValid(futureDate);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsValid_WithFutureDateAndCanEqualsFalse_ShouldReturnTrue()
    {
        // Arrange
        var attribute = new FutureDateAttribute();
        var futureDate = DateTime.Now.AddDays(1);

        // Act
        var result = attribute.IsValid(futureDate);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsValid_WithCurrentDateAndCanEqualsTrue_ShouldReturnTrue()
    {
        // Arrange
        var attribute = new FutureDateAttribute { CanEquals = true };
        var currentDate = DateTime.Now;

        // Act
        var result = attribute.IsValid(currentDate);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsValid_WithCurrentDateAndCanEqualsFalse_ShouldReturnFalse()
    {
        // Arrange
        var attribute = new FutureDateAttribute();
        var currentDate = DateTime.Now;

        // Act
        var result = attribute.IsValid(currentDate);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsValid_WithPastDateAndCanEqualsTrue_ShouldReturnFalse()
    {
        // Arrange
        var attribute = new FutureDateAttribute { CanEquals = true };
        var pastDate = DateTime.Now.AddDays(-1);

        // Act
        var result = attribute.IsValid(pastDate);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void IsValid_WithPastDateAndCanEqualsFalse_ShouldReturnFalse()
    {
        // Arrange
        var attribute = new FutureDateAttribute();
        var pastDate = DateTime.Now.AddDays(-1);

        // Act
        var result = attribute.IsValid(pastDate);

        // Assert
        Assert.IsFalse(result);
    }
}
