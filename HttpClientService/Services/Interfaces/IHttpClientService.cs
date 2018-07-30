using System.Threading.Tasks;
using HttpClientService.Models.Interfaces;
using HttpClientService.Requests.Interfaces;

namespace HttpClientService.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<IHttpResponse<string>> RequestAsync(IRequest request);
        Task<IHttpResponse<T>> RequestAsync<T>(IRequest request);
    }    
}