using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ndknitor.System;

[TestFixture]
public class ExtensionsTests
{
    [Test]
    public void MapProperty_MapsPropertiesCorrectly()
    {
        // Arrange
        var sourceArray = new List<int> { 30, 25, 35 };
        var targetArray = new List<Person>
        {
            new Person { Age = 0 },
            new Person { Age = 0 },
            new Person { Age = 0 }
        };

        // Act
        targetArray.MapProperty(item => item.Age, sourceArray);

        // Assert
        Assert.That(sourceArray[0], Is.EqualTo(targetArray[0].Age));
        Assert.That(sourceArray[1], Is.EqualTo(targetArray[1].Age));
        Assert.That(sourceArray[2], Is.EqualTo(targetArray[2].Age));
    }

    [Test]
    public void MapProperty_ThrowsArgumentNullException_WhenTargetArrayIsNull()
    {
        // Arrange
        IEnumerable<Person> targetArray = null;
        var sourceArray = new List<int> { 30, 25, 35 };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            targetArray.MapProperty(item => item.Age, sourceArray);
        });
    }

    [Test]
    public void MapProperty_ThrowsArgumentNullException_WhenPropertyExpressionIsNull()
    {
        // Arrange
        var targetArray = new List<Person>();
        IEnumerable<int> sourceArray = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            targetArray.MapProperty(null, sourceArray);
        });
    }

    [Test]
    public void MapProperty_ThrowsArgumentNullException_WhenSourceArrayIsNull()
    {
        // Arrange
        var targetArray = new List<Person>();
        var sourceArray = new List<int> { 30, 25, 35 };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            targetArray.MapProperty(item => item.Age, null);
        });
    }

    [Test]
    public void MapProperty_ThrowsArgumentException_WhenArrayLengthsDiffer()
    {
        // Arrange
        var targetArray = new List<Person> { new Person() };
        var sourceArray = new List<int> { 30, 25, 35 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            targetArray.MapProperty(item => item.Age, sourceArray);
        });
    }

    private class Person
    {
        public int Age { get; set; }
    }
}
