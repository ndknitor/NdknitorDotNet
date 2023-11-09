using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Ndknitor.Web.Validations;

[TestFixture]
public class ClassPropertyAttributeTests
{
    [Test]
    public void IsValid_WithValidSinglePropertyName_ShouldReturnSuccess()
    {
        // Arrange
        var targetType = typeof(TestClass);
        var attribute = new ClassPropertyAttribute(targetType);

        // Act
        var result = attribute.GetValidationResult("Name", new ValidationContext(null));

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void IsValid_WithValidArrayPropertyNames_ShouldReturnSuccess()
    {
        // Arrange
        var targetType = typeof(TestClass);
        var attribute = new ClassPropertyAttribute(targetType);

        // Act
        var result = attribute.GetValidationResult(new string[] { "Name", "Age" }, new ValidationContext(null));

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void IsValid_WithInvalidPropertyName_ShouldReturnValidationError()
    {
        // Arrange
        var targetType = typeof(TestClass);
        var attribute = new ClassPropertyAttribute(targetType);

        // Act
        var result = attribute.GetValidationResult("InvalidProperty", new ValidationContext(null));

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.ErrorMessage, Is.EqualTo("InvalidProperty is not a valid property of TestClass."));
    }

    [Test]
    public void IsValid_WithNullValue_ShouldReturnSuccess()
    {
        // Arrange
        var targetType = typeof(TestClass);
        var attribute = new ClassPropertyAttribute(targetType);

        // Act
        var result = attribute.GetValidationResult(null, new ValidationContext(null));

        // Assert
        Assert.IsNull(result);
    }
}

public class TestClass
{
    public string Name { get; set; }
    public int Age { get; set; }
}
