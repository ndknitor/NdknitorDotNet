using NUnit.Framework;
using Newtonsoft.Json;
using Ndknitor.System;

[TestFixture]
public class StringExtensionTests
{
    [Test]
    public void ToJsonClass_DeserializesStringToJson()
    {
        // Arrange
        string jsonString = "{\"Name\":\"John\",\"Age\":30}";

        // Act
        var obj = jsonString.ToJsonClass<Person>();

        // Assert
        Assert.IsNotNull(obj);
        Assert.IsInstanceOf<Person>(obj);
        Assert.That(obj.Name, Is.EqualTo("John"));
        Assert.That(obj.Age, Is.EqualTo(30));
    }

    [Test]
    public void ToJsonClass_DeserializesStringToObjectType()
    {
        // Arrange
        string jsonString = "{\"Name\":\"Alice\",\"Age\":25}";

        // Act
        var obj = jsonString.ToJsonClass<object>();

        // Assert
        Assert.IsNotNull(obj);
        Assert.IsInstanceOf<object>(obj);
    }

    [Test]
    public void ToJsonClass_ThrowsJsonException_WhenStringIsInvalidJson()
    {
        // Arrange
        string invalidJsonString = "Invalid JSON";

        // Act & Assert
        Assert.Throws<JsonReaderException>(() =>
        {
            invalidJsonString.ToJsonClass<Person>();
        });
    }
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
