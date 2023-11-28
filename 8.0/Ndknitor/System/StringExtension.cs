using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
namespace Ndknitor.System;
public static class StringExtension
{
    /// <summary>
    /// Deserialize a JSON string into an instance of a specified class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <returns></returns>
    public static T ToJsonClass<T>(this string str)
    {
        return JsonConvert.DeserializeObject<T>(str);
    }
}
