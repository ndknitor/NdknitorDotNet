using Newtonsoft.Json;
namespace Ndknitor.System;
public static class StringExtension
{
    public static T ToClass<T>(this string str)
    {
        return JsonConvert.DeserializeObject<T>(str);
    }
}
