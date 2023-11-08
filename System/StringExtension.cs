using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
namespace Ndknitor.System;
public static class StringExtension
{
    public static T ToJsonClass<T>(this string str)
    {
        return JsonConvert.DeserializeObject<T>(str);
    }
    public static T ToBsonClass<T>(this string base64data)
    {
        byte[] data = Convert.FromBase64String(base64data);

        using (MemoryStream ms = new MemoryStream(data))
        using (BsonDataReader reader = new BsonDataReader(ms))
        {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<T>(reader);
        }
    }
}
