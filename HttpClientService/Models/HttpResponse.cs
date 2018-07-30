using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using HttpClientService.Constants;
using HttpClientService.Helpers;
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
                        return XmlHelper.DeserializeXml<T>(content);    
                    default:
                        return JsonConvert.DeserializeObject<T>(content);
            }
        }

        
    }
}