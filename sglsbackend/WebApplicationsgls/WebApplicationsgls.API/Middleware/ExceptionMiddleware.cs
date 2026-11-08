using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApplicationsgls.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            int status;
            string title;
            string detail = exception.Message;

            switch (exception)
            {
                case ApiException apiEx:
                    status = apiEx.StatusCode;
                    title = apiEx.Title ?? "Erro na requisição";
                    break;
                case UnauthorizedAccessException _:
                    status = (int)HttpStatusCode.Unauthorized;
                    title = "Acesso não autorizado";
                    break;
                case ArgumentException _:
                    status = (int)HttpStatusCode.BadRequest;
                    title = "Requisição inválida";
                    break;
                default:
                    status = (int)HttpStatusCode.InternalServerError;
                    title = "Erro interno do servidor";
                    break;
            }

            _logger.LogError(exception, "Unhandled exception (TraceId: {TraceId})", traceId);

            var problem = new
            {
                type = $"https://httpstatuses.io/{status}",
                title,
                status,
                detail,
                traceId
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = status;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
        }
    }
}