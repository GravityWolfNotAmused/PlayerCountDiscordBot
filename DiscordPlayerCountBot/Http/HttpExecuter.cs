using DiscordPlayerCountBot.Json;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Http
{
    public class HttpExecuter : IHttpExecuter, IDisposable
    {
        public readonly HttpClient HttpClient;

        public HttpExecuter(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        
        public void Dispose()
        {
            HttpClient?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<TResponse?> GET<TRequest, TResponse>(string endPoint, IQueryParameterBuilder? queryBuilder = null, TRequest? body = default)
        {
            return await ExecuteHttpRequest<TRequest, TResponse>(endPoint, HttpMethod.Get, queryBuilder, body);
        }

        public async Task<TResponse?> POST<TRequest, TResponse>(string endPoint, IQueryParameterBuilder? queryBuilder = null, TRequest? body = default)
        {
            return await ExecuteHttpRequest<TRequest, TResponse>(endPoint, HttpMethod.Post, queryBuilder, body);
        }

        public async Task<TResponse?> PATCH<TRequest, TResponse>(string endPoint, IQueryParameterBuilder? queryBuilder = null, TRequest? body = default)
        {
            return await ExecuteHttpRequest<TRequest, TResponse>(endPoint, HttpMethod.Patch, queryBuilder, body);
        }

        public async Task<TResponse?> PUT<TRequest, TResponse>(string endPoint, IQueryParameterBuilder? queryBuilder = null, TRequest? body = default)
        {
            return await ExecuteHttpRequest<TRequest, TResponse>(endPoint, HttpMethod.Put, queryBuilder, body);
        }

        public async Task<TResponse?> DELETE<TRequest, TResponse>(string endPoint, IQueryParameterBuilder? queryBuilder = null, TRequest? body = default)
        {
            return await ExecuteHttpRequest<TRequest, TResponse>(endPoint, HttpMethod.Delete, queryBuilder, body);
        }

        private async Task<TResponse?> ExecuteHttpRequest<TRequest, TResponse>(string endPoint, HttpMethod method, IQueryParameterBuilder? queryBuilder = null, TRequest? body = default)
        {
            if (HttpClient == null) return default;

            var queryParams = queryBuilder?.CreateQueryParameterString();
            var fullPath = $"{endPoint}{queryParams}";

            using var request = new HttpRequestMessage(method, fullPath);

            if (body != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonHelper.DeserializeObject<TResponse>(jsonString);
        }
    }
}
