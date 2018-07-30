using System;
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
        public static IServiceCollection RegisterHttpClientService(this IServiceCollection services, int timeOutTimeInSeconds = 10, 
            int numberOfRetries = 3)
        {
            services.AddHttpClient<IHttpClientService, Services.HttpClientService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(request =>
                    Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(timeOutTimeInSeconds)))
                .AddPolicyHandler(GetRetryPolicy(numberOfRetries))
                .AddPolicyHandler(GetCircuitBreakerPolicy());
            
            return services.AddSingleton<IHttpClientService, Services.HttpClientService>();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int numberOfRetries)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(message => message.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(numberOfRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}