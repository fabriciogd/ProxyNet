namespace ProxyNet.Serializer
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.IO;

    public class ProxyRequestSerializer : IProxyRequestSerializer
    {
        public void Serialize(Stream stream, object[] parameters)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "dd-MMM-yyyy" });
            settings.Formatting = Formatting.Indented;

            string json = JsonConvert.SerializeObject(parameters, settings);

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(json);
            writer.Flush();
        }
    }
}
