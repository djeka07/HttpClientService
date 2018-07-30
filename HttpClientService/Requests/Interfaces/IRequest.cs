using System.Collections.Generic;
using HttpClientService.Models.Interfaces;
using HttpMethod = HttpClientService.Enums.HttpMethod;

namespace HttpClientService.Requests.Interfaces
{
    public interface IRequest
    {
        string Url { get; }
        HttpMethod Method { get; }
        IContent Content { get; }
        IEnumerable<KeyValuePair<string, string>> Headers { get; }
    }
}