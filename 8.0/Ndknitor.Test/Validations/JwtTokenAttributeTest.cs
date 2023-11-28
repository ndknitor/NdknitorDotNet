using System.ComponentModel.DataAnnotations;
using Ndknitor.Web.Validations;
using NUnit.Framework;

[TestFixture]
public class JwtTokenAttributeTests
{
    [Test]
    public void ValidJwtToken_ShouldPassValidation()
    {
        // Arrange
        var jwtTokenAttribute = new JwtTokenAttribute();
        var validJwt = "eyJabc.def.ghi";

        // Act
        var validationResult = jwtTokenAttribute.GetValidationResult(validJwt, new ValidationContext(validJwt));

        // Assert
        Assert.That(validationResult, Is.EqualTo(ValidationResult.Success));
    }

    [Test]
    public void NullJwtToken_ShouldPassValidation()
    {
        // Arrange
        var jwtTokenAttribute = new JwtTokenAttribute();

        // Act
        var validationResult = jwtTokenAttribute.GetValidationResult(null, new ValidationContext(""));

        // Assert
        Assert.That(validationResult, Is.EqualTo(ValidationResult.Success));
    }

    [Test]
    public void InvalidJwtToken_ShouldFailValidation()
    {
        // Arrange
        var jwtTokenAttribute = new JwtTokenAttribute();
        var invalidJwt = "invalid_jwt";

        // Act
        var validationResult = jwtTokenAttribute.GetValidationResult(invalidJwt, new ValidationContext(invalidJwt));

        // Assert
        Assert.That(validationResult.ErrorMessage, Is.EqualTo("Invalid JWT format."));
    }
}
