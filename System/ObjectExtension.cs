using Newtonsoft.Json;
namespace Ndknitor.System;
public static class ObjectExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
}
