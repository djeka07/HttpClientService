using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HttpClientService.Constants;
using HttpClientService.Models;
using HttpClientService.Models.Interfaces;
using HttpClientService.Requests.Interfaces;
using HttpClientService.Services.Interfaces;

namespace HttpClientService.Services
{
    public class HttpClientService: IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IHttpResponse<string>> RequestAsync(IRequest request)
        {
            var result = await InternalRequest(request).ConfigureAwait(false);
            return await CreateResponse<string>(result).ConfigureAwait(false);
        }


        public async Task<IHttpResponse<T>> RequestAsync<T>(IRequest request)
        {
            var result = await InternalRequest(request).ConfigureAwait(false);
            return await CreateResponse<T>(result).ConfigureAwait(false);
        }

        private async Task<IHttpResponse<T>> CreateResponse<T>(HttpResponseMessage result)
        {
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponse<T>(result.IsSuccessStatusCode, result.StatusCode, content, result.Content.Headers.ContentType.MediaType);
        }

        private async Task<HttpResponseMessage> InternalRequest(IRequest request)
        {
            ValidateRequest(request);
            var requestMessage = CreateHttpRequestMessage(request);
            var result = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            return result;
        }

        private HttpRequestMessage CreateHttpRequestMessage(IRequest request)
        {
            var method = GetMethod(request.Method);
            var requestMessage = new HttpRequestMessage(method, request.Url);

            if (request.Headers != null && request.Headers.Any())
            {
                foreach (var requestHeader in request.Headers)
                {
                    requestMessage.Headers.Add(requestHeader.Key, requestHeader.Value);
                }
            }

            if (request.Content != null)
                requestMessage.Content = new StringContent(request.Content.Serialize(), Encoding.UTF8, request.Content.MediaType);

            return requestMessage;
        }

        private HttpMethod GetMethod(Enums.HttpMethod requestMethod)
        {
            switch (requestMethod)
            {
                    case Enums.HttpMethod.GET:
                        return HttpMethod.Get;
                    case Enums.HttpMethod.POST:
                        return HttpMethod.Post;
                    case Enums.HttpMethod.PUT:
                    case Enums.HttpMethod.PATCH:
                        return HttpMethod.Put;
                    case Enums.HttpMethod.DELETE:
                        return HttpMethod.Delete;
                    default:
                        throw new Exception($"Unknown method: {requestMethod}");
                        
            }
        }
        
        private void ValidateRequest(IRequest request)
        {
            if (request == null) throw new ArgumentNullException("request is null");
            if (string.IsNullOrEmpty(request.Url)) throw new ArgumentNullException("url is string or empty");
            if (request.Method == Enums.HttpMethod.NOTSET) throw new ArgumentException("method must be set");
        }
    }
}