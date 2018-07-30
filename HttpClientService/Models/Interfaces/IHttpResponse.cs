using System.Net;

namespace HttpClientService.Models.Interfaces
{
    public interface IHttpResponse<T>
    {
        bool Success { get; }
        HttpStatusCode StatusCode { get; }
        T Content { get; }
    }
}