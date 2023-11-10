using System.ComponentModel.DataAnnotations;
using Ndknitor.System;
using Ndknitor.Web.Validations;
using StackExchange.Redis;
var regularTextAttribute = new RegularTextAttribute
{
    IncludeSpace = true,
    IncludeNumber = false,
    IncludeCharaters = "!@#"
};

// Act
bool isValid = regularTextAttribute.IsValid("Valid_Text");
Console.WriteLine(isValid);



public class TestModel
{
    public int Value { get; set; }
    public int ComparisonValue { get; set; }
}

//Console.WriteLine(result.ErrorMessage);


// var obj = new
// {
//     Name = "Movie Premiere",
//     StartDate = new DateTime(2013, 1, 22, 20, 30, 0, DateTimeKind.Utc),
//     Description = "Hello"
// };
// using (var multiplexer = ConnectionMultiplexer.Connect("localhost:6379"))
// {
//     var db = multiplexer.GetDatabase();
//     db.StringSet("1", obj.ToJson(), TimeSpan.FromSeconds(69));
//     db.StringSet("2", obj.ToBson(), TimeSpan.FromSeconds(69));
// }
// string json = obj.ToJson();

// Console.WriteLine($"{json} : {json.Length}");
// Console.WriteLine(obj.ToBson().Length);
