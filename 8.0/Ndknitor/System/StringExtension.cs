using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
namespace Ndknitor.System;
public static class StringExtension
{
    public static T ToJsonClass<T>(this string str)
    {
        return JsonConvert.DeserializeObject<T>(str);
    }
}
