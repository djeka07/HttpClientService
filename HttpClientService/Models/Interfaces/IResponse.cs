using System.Net;

namespace HttpClientService.Models.Interfaces
{
    public interface IResponse<T>
    {
        bool Success { get; }
        HttpStatusCode StatusCode { get; }
        T Content { get; }
    }
}