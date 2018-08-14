using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using HttpClientService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace HttpClientService
{
    public static class Initialize
    {
        public static IServiceCollection AddHttpClientService(this IServiceCollection services, IEnumerable<IAsyncPolicy<HttpResponseMessage>> policies = null,
            int? handlerLifeTimeInMinutes = null)
        {
            var httpClientBuilder = services.AddHttpClient<IHttpClientService, Services.HttpClientService>();

            if (policies != null) 
                httpClientBuilder = AddPolicyHandlers(httpClientBuilder, policies);
            
            if (handlerLifeTimeInMinutes.HasValue)
                httpClientBuilder.SetHandlerLifetime(TimeSpan.FromMinutes(handlerLifeTimeInMinutes.Value));
                
            return services.AddSingleton<IHttpClientService, Services.HttpClientService>();
        }

        private static IHttpClientBuilder AddPolicyHandlers(IHttpClientBuilder httpClientBuilder, 
            IEnumerable<IAsyncPolicy<HttpResponseMessage>> policies)
        {
            foreach (var asyncPolicy in policies)
            {
                httpClientBuilder.AddPolicyHandler(asyncPolicy);
            }

            return httpClientBuilder;
        }
    }
}