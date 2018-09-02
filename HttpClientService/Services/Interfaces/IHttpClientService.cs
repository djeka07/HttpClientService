using System.Threading.Tasks;
using HttpClientService.Models.Interfaces;
using HttpClientService.Requests.Interfaces;

namespace HttpClientService.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<IResponse<string>> RequestAsync(IRequest request);
        Task<IResponse<T>> RequestAsync<T>(IRequest request);
    }    
}