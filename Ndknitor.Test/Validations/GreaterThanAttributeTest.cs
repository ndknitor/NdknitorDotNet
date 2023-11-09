using Ndknitor.Web.Validations;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class GreaterThanAttributeTests
{
    [Test]
    public void IsValid_WithGreaterValueAndCanEqualsTrue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new GreaterThanAttribute("ComparisonValue") { CanEquals = true };
        var model = new TestModel { Value = 5, ComparisonValue = 3 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void IsValid_WithGreaterValueAndCanEqualsFalse_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new GreaterThanAttribute("ComparisonValue");
        var model = new TestModel { Value = 5, ComparisonValue = 3 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void IsValid_WithEqualValueAndCanEqualsTrue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new GreaterThanAttribute("ComparisonValue") { CanEquals = true };
        var model = new TestModel { Value = 5, ComparisonValue = 5 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void IsValid_WithEqualValueAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new GreaterThanAttribute("ComparisonValue");
        var model = new TestModel { Value = 5, ComparisonValue = 5 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.ErrorMessage, Is.EqualTo("TestModel must be greater than ComparisonValue"));
    }

    [Test]
    public void IsValid_WithSmallerValueAndCanEqualsTrue_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new GreaterThanAttribute("ComparisonValue") { CanEquals = true };
        var model = new TestModel { Value = 3, ComparisonValue = 5 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.ErrorMessage, Is.EqualTo("TestModel must be equals or greater than ComparisonValue"));
    }

    [Test]
    public void IsValid_WithSmallerValueAndCanEqualsFalse_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new GreaterThanAttribute("ComparisonValue");
        var model = new TestModel { Value = 3, ComparisonValue = 5 };

        // Act
        var result = attribute.GetValidationResult(model.Value, new ValidationContext(model));

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.ErrorMessage, Is.EqualTo("TestModel must be greater than ComparisonValue"));
    }
}

public class TestModel
{
    public int Value { get; set; }
    public int ComparisonValue { get; set; }
}
