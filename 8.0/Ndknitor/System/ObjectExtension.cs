using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
namespace Ndknitor.System;
public static class ObjectExtensions
{
    /// <summary>
    /// Serialize an object to its JSON representation.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    /// <summary>
    /// Convert an object into its BSON (Binary JSON) representation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Convert a byte array containing BSON (Binary JSON) data into an instance of a specified class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
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
