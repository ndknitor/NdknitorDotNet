using Ndknitor.Web.Validations;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class LessThanAttributeTests
{
    [Test]
    public void IsValid_WithSmallerValueAndCanEqualsTrue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new LessThanAttribute("ComparisonValue") { CanEquals = true };
        var model = new TestModel { Value = 3, ComparisonValue = 5 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithSmallerValueAndCanEqualsFalse_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new LessThanAttribute("ComparisonValue");
        var model = new TestModel { Value = 3, ComparisonValue = 5 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithEqualValueAndCanEqualsTrue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new LessThanAttribute("ComparisonValue") { CanEquals = true };
        var model = new TestModel { Value = 5, ComparisonValue = 5 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithEqualValueAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new LessThanAttribute("ComparisonValue");
        var model = new TestModel { Value = 5, ComparisonValue = 5 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("TestModel must be less than ComparisonValue"));
    }

    [Test]
    public void IsValid_WithGreaterValueAndCanEqualsTrue_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new LessThanAttribute("ComparisonValue") { CanEquals = true };
        var model = new TestModel { Value = 5, ComparisonValue = 3 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("TestModel must be equals or less than ComparisonValue"));
    }

    [Test]
    public void IsValid_WithGreaterValueAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new LessThanAttribute("ComparisonValue");
        var model = new TestModel { Value = 5, ComparisonValue = 3 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("TestModel must be less than ComparisonValue"));
    }
}
