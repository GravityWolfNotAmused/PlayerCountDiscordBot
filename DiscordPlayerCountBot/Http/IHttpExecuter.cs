using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Http
{
    public interface IHttpExecuter
    {
        Task<TResponse?> DELETE<TRequest, TResponse>(string endPoint, IQueryParameterBuilder queryBuilder, TRequest? body);
        Task<TResponse?> GET<TRequest, TResponse>(string endPoint, IQueryParameterBuilder queryBuilder, TRequest? body);
        Task<TResponse?> PATCH<TRequest, TResponse>(string endPoint, IQueryParameterBuilder queryBuilder, TRequest? body);
        Task<TResponse?> POST<TRequest, TResponse>(string endPoint, IQueryParameterBuilder queryBuilder, TRequest? body);
        Task<TResponse?> PUT<TRequest, TResponse>(string endPoint, IQueryParameterBuilder queryBuilder, TRequest? body);
    }
}