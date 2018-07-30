using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HttpClientService.Helpers
{
    public static class XmlHelper
    {
        public static T DeserializeXml<T>(string content)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = XmlReader.Create(new StringReader(content)))
            {
                if (!serializer.CanDeserialize(reader))
                    throw new XmlException($"Cannot deserialize content of type {typeof(T)}");
                
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}