using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
namespace Ndknitor.System;
public static class ObjectExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    public static byte[] ToBson(this object value)
    {
        using (MemoryStream ms = new MemoryStream())
        using (BsonDataWriter datawriter = new BsonDataWriter(ms))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(datawriter, value);
            return ms.ToArray();
        }
    }
    public static T ToBsonClass<T>(this byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        using (BsonDataReader reader = new BsonDataReader(ms))
        {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<T>(reader);
        }
    }
}
