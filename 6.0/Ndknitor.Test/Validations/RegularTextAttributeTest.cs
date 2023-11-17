using NUnit.Framework;
using System.IO;

[TestFixture]
public class RegularTextAttributeTests
{
    [TestCase("ValidText", true, false, "-_", ExpectedResult = true)]
    [TestCase("Invalid123", true, false, "-_", ExpectedResult = false)]

    [TestCase(null, true, false, "-_", ExpectedResult = true)]

    [TestCase("Valid_Text", true, true, "-_", ExpectedResult = true)]
    [TestCase("Invalid_Text", true, true, "-", ExpectedResult = false)]

    [TestCase("Invalid 123", false, true, "-_", ExpectedResult = false)]
    [TestCase("Valid123", false, true, "-_", ExpectedResult = true)]

    [TestCase("Valid_Text", false, false, "-_", ExpectedResult = true)]
    [TestCase("Valid_Text1234", false, false, "-_", ExpectedResult = false)]


    [TestCase("Valid_Text", true, false, "!@#", ExpectedResult = false)]
    [TestCase("Valid!Text", true, false, "!@#", ExpectedResult = true)]
    public object IsValid_WithDifferentCases(string value, bool includeSpace, bool includeNumber, string includeCharacters)
    {
        // Arrange
        var regularTextAttribute = new RegularTextAttribute
        {
            IncludeSpace = includeSpace,
            IncludeNumber = includeNumber,
            IncludeCharaters = includeCharacters
        };
        return regularTextAttribute.IsValid(value);
    }

    [Test]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        // Arrange
        var regularTextAttribute = new RegularTextAttribute();
        string propertyName = "MyProperty";

        // Act
        string errorMessage = regularTextAttribute.FormatErrorMessage(propertyName);

        // Assert
        Assert.That(errorMessage, Is.EqualTo($"{propertyName} was not in the correct format"));
    }
}
