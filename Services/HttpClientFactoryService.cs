using EdgeCtrlClient.DTOs;
using EdgeCtrlClient.DTOs.EdgeController;
using EdgeCtrlClient.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EdgeCtrlClient.Services
{
    public interface IHttpClientFactoryService
    {
        //Task<EdgeApiResponse> SendRequestAsync(HttpMethod method, string url, object? content = null);

        Task<EdgePlusApiResponse<T>> SendRequestAsync<T>(HttpMethod method, string url, object? content = null);
    }

    internal class HttpClientFactoryService : IHttpClientFactoryService
    {
        private static readonly int maxRetries = 3;
        private static readonly TimeSpan retryDelay = TimeSpan.FromSeconds(1);

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.ServiceUnavailable)
            .WaitAndRetryAsync(maxRetries, retryAttempt => retryDelay);

        public HttpClientFactoryService()
        {
            _httpClientFactory = CreateHttpClientFactory();
            _logger = LoggerHelper.GetDefaultLogger();
        }

        private static IHttpClientFactory CreateHttpClientFactory()
        {
            var services = new ServiceCollection();

            services.AddHttpClient("CustomClient")
                    .ConfigurePrimaryHttpMessageHandler(() =>
                        new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                (message, cert, chain, errors) => true
                        });

            return services.BuildServiceProvider()
                           .GetRequiredService<IHttpClientFactory>();
        }

        /*
        public async Task<EdgeApiResponse> SendRequestAsync(HttpMethod method, string url, object? content = null)
        {
            // Create the request message
            var requestMessage = new HttpRequestMessage(method, url);

            if (content is byte[] fileBytes)
            {
                requestMessage.Content = new ByteArrayContent(fileBytes);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            else if (content != null)
            {
                // If content is provided, serialize it to JSON and set it as the request content
                requestMessage.Content = new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json"
                );
                //_logger.Information(JsonConvert.SerializeObject(content));
            }

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("CustomClient");

                //var cookiesInRequest = _cookieContainer.GetCookies(new Uri(url));
                //foreach (Cookie cookie in cookiesInRequest)
                //{
                //    _logger.LogInformation($"[Cookie in Store] {cookie.Name} = {cookie.Value}");
                //}

                // Send the request
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                //var cookiesInResponse = _cookieContainer.GetCookies(new Uri(url));
                //foreach (Cookie cookie in cookiesInResponse)
                //{
                //    _logger.LogInformation($"[Cookie in Store] {cookie.Name} = {cookie.Value}");
                //}

                // Read the response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Return the status code and content in the ApiResponse
                return new EdgeApiResponse(response.StatusCode, responseContent);
            }
            catch (HttpRequestException ex)
            {
                // Handle specific HTTP request exceptions
                _logger.Error(ex, "[SendRequest] Send Request failed.");
                return new EdgeApiResponse(HttpStatusCode.InternalServerError, $"Request failed: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Handle timeout exceptions
                _logger.Error("[SendRequest] Request timed out.");
                return new EdgeApiResponse(HttpStatusCode.RequestTimeout, "Request timed out.");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                _logger.Error(ex, "[SendRequest] An unexpected error occurred.");
                return new EdgeApiResponse(HttpStatusCode.InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }
        */

        public async Task<EdgePlusApiResponse<T>> SendRequestAsync<T>(HttpMethod method, string url, object? content = null)
        {
            // Create the request message
            var requestMessage = new HttpRequestMessage(method, url);

            if (content is byte[] fileBytes)
            {
                requestMessage.Content = new ByteArrayContent(fileBytes);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            else if (content != null)
            {
                // If content is provided, serialize it to JSON and set it as the request content
                requestMessage.Content = new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json"
                );
                //_logger.Information(JsonConvert.SerializeObject(content));
            }

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("CustomClient");

                //var cookiesInRequest = _cookieContainer.GetCookies(new Uri(url));
                //foreach (Cookie cookie in cookiesInRequest)
                //{
                //    _logger.LogInformation($"[Cookie in Store] {cookie.Name} = {cookie.Value}");
                //}

                // Send the request
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                //var cookiesInResponse = _cookieContainer.GetCookies(new Uri(url));
                //foreach (Cookie cookie in cookiesInResponse)
                //{
                //    _logger.LogInformation($"[Cookie in Store] {cookie.Name} = {cookie.Value}");
                //}

                // Read the response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Return the status code and content in the ApiResponse
                //return new EdgeApiResponse(response.StatusCode, responseContent);

                string trimmed = responseContent.Trim();
                if(!(trimmed.StartsWith("{") || trimmed.StartsWith("[")))
{
                    return new EdgePlusApiResponse<T>
                    {
                        Payload = new Payload
                        {
                            ErrCode = response.IsSuccessStatusCode ? 1 : 0,
                            Info = responseContent
                        }
                    };
                }

                return string.IsNullOrWhiteSpace(responseContent)
                    ? new EdgePlusApiResponse<T>()
                    : JsonConvert.DeserializeObject<EdgePlusApiResponse<T>>(responseContent) ?? new EdgePlusApiResponse<T>();

            }
            catch (HttpRequestException ex)
            {
                // Handle specific HTTP request exceptions
                _logger.Error(ex, "[SendRequest] Send Request failed.");
                //return new EdgeApiResponse(HttpStatusCode.InternalServerError, $"Request failed: {ex.Message}");

                return GetExceptionMessage<T>(string.Format("Request failed: {0}", ex.Message));
            }
            catch (TaskCanceledException)
            {
                // Handle timeout exceptions
                _logger.Error("[SendRequest] Request timed out.");
                //return new EdgeApiResponse(HttpStatusCode.RequestTimeout, "Request timed out.");
                return GetExceptionMessage<T>("Request timed out."); ;
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                _logger.Error(ex, "[SendRequest] An unexpected error occurred.");
                //return new EdgeApiResponse(HttpStatusCode.InternalServerError, $"An unexpected error occurred: {ex.Message}");
                return GetExceptionMessage<T>(string.Format("Request failed: {0}", ex.Message));
            }
        }

        private static EdgePlusApiResponse<T> GetExceptionMessage<T>(string message)
        {
            return new EdgePlusApiResponse<T>()
            {
                Payload = new Payload()
                {
                    ErrCode = 0,
                    Info = message
                },
                Info = default
            };
        }
    }
}
