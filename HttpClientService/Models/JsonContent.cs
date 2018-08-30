using HttpClientService.Models.Interfaces;
using Newtonsoft.Json;

namespace HttpClientService.Models
{
    public class JsonContent: IContent
    {
            public JsonContent(object body)
        {
            Body = body;
            MediaType = Constants.MediaType.Json;
        }

        public object Body { get; }
        public string MediaType { get; }
        
        public string Serialize()
        {
            return Body != null 
                ? JsonConvert.SerializeObject(Body)
                : string.Empty;
        }
    }
}