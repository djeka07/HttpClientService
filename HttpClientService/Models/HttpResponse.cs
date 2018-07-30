using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using HttpClientService.Constants;
using HttpClientService.Models.Interfaces;
using Newtonsoft.Json;

namespace HttpClientService.Models
{
    public class HttpResponse<T>: IHttpResponse<T>
    {
        public HttpResponse(bool success, HttpStatusCode statusCode, string content, string mediaType)
        {
            Success = success;
            StatusCode = statusCode;
            Content = Deserialize(content, mediaType);
        }
        
        public bool Success { get; }
        public HttpStatusCode StatusCode { get; }
        public T Content { get; }
        
        private T Deserialize(string content, string mediaType)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)content;
            
            switch (mediaType)
            {
                    case MediaType.TextXml:
                    case MediaType.Xml:
                        return DeserializeXml(content);    
                    default:
                        return JsonConvert.DeserializeObject<T>(content);
            }
        }

        private T DeserializeXml(string content)
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