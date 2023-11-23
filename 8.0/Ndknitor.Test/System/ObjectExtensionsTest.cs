using NUnit.Framework;
using Newtonsoft.Json;
using System.IO;
using Ndknitor.System;

[TestFixture]
public class ObjectExtensionsTests
{
    [Test]
    public void ToJson_SerializesObjectToJson()
    {
        // Arrange
        var obj = new { Name = "John", Age = 30 };

        // Act
        string json = obj.ToJson();

        // Assert
        Assert.That(json, Is.EqualTo("{\"Name\":\"John\",\"Age\":30}"));
    }

    [Test]
    public void ToBson_SerializesObjectToBson()
    {
        // Arrange
        var obj = new { Name = "Alice", Age = 25 };

        // Act
        byte[] bson = obj.ToBson();

        // Assert
        Assert.IsNotNull(bson);
        Assert.IsTrue(bson.Length > 0);
    }

    [Test]
    public void ToBsonClass_DeserializesBsonToObjectType()
    {
        // Arrange
        var obj = new { Name = "Bob", Age = 35 };
        byte[] bson = obj.ToBson();

        // Act
        var deserializedObj = bson.ToBsonClass<object>();

        // Assert
        Assert.IsNotNull(deserializedObj);
        Assert.IsInstanceOf<object>(deserializedObj);
    }
}
