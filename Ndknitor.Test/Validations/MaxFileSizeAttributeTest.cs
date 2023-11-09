using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Ndknitor.Web.Validations;

[TestFixture]
public class MaxFileSizeAttributeTests
{
    [Test]
    public void IsValid_WithFileSizeLessThanMax_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new MaxFileSizeAttribute(5); // 5 MB
        var file = new FormFile(new MemoryStream(new byte[4 * 1024 * 1024]), 0, 4 * 1024 * 1024, "testFile", "test.txt");

        // Act
        var result = attribute.GetValidationResult(file, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithFileSizeEqualToMax_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new MaxFileSizeAttribute(5); // 5 MB
        var file = new FormFile(new MemoryStream(new byte[5 * 1024 * 1024]), 0, 5 * 1024 * 1024, "testFile", "test.txt");

        // Act
        var result = attribute.GetValidationResult(file, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void IsValid_WithFileSizeGreaterThanMax_ShouldReturnValidationError()
    {
        // Arrange
        var attribute = new MaxFileSizeAttribute(5); // 5 MB
        var file = new FormFile(new MemoryStream(new byte[6 * 1024 * 1024]), 0, 6 * 1024 * 1024, "testFile", "test.txt");

        // Act
        var result = attribute.GetValidationResult(file, new ValidationContext(null));

        // Assert
        Assert.That(result.ErrorMessage, Is.EqualTo("The file size cannot exceed 5 MB."));
    }

    [Test]
    public void IsValid_WithNullValue_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new MaxFileSizeAttribute(5); // 5 MB

        // Act
        var result = attribute.GetValidationResult(null, new ValidationContext(null));

        // Assert
        Assert.That(result, Is.Null);
    }
}
