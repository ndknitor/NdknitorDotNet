using Microsoft.AspNetCore.Http;
using Ndknitor.Web.Validations;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

[TestFixture]
public class AllowedExtensionsAttributeTests
{
    [Test]
    public void IsValid_WithValidExtension_ShouldReturnSuccess()
    {
        // Arrange
        var allowedExtensions = new string[] { ".jpg", ".png" };
        var attribute = new AllowedExtensionsAttribute(allowedExtensions);
        var file = new TestFormFile("test.jpg");

        // Act
        var result = attribute.GetValidationResult(file, new ValidationContext(file));

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void IsValid_WithInvalidExtension_ShouldReturnValidationError()
    {
        // Arrange
        var allowedExtensions = new string[] { ".jpg", ".png" };
        var attribute = new AllowedExtensionsAttribute(allowedExtensions);
        var file = new TestFormFile("test.txt");

        // Act
        var result = attribute.GetValidationResult(file, new ValidationContext(file));

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.ErrorMessage, Is.EqualTo("Allowed file extensions are: .jpg, .png"));
    }

    [Test]
    public void IsValid_WithNullValue_ShouldReturnSuccess()
    {
        // Arrange
        var allowedExtensions = new string[] { ".jpg", ".png" };
        var attribute = new AllowedExtensionsAttribute(allowedExtensions);

        // Act
        var result = attribute.GetValidationResult(null, new ValidationContext(null));

        // Assert
        Assert.IsNull(result);
    }
}

public class TestFormFile : IFormFile
{
    private readonly string fileName;

    public TestFormFile(string fileName)
    {
        this.fileName = fileName;
    }

    public string ContentType => "text/plain";
    public string ContentDisposition => null;
    public IHeaderDictionary Headers => null;
    public long Length => 0;
    public string Name => null;

    public string FileName => fileName;

    public void CopyTo(Stream target)
    {
    }

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Stream OpenReadStream()
    {
        return new MemoryStream();
    }
}
