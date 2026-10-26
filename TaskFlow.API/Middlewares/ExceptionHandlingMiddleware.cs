using System.Net;
using System.Text.Json;
using TaskFlow.API.Exceptions;

namespace TaskFlow.API.Middlewares;

/// <summary>Captura exceções não tratadas e devolve JSON padronizado, sem stack trace.</summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            // o stack trace fica somente no log do servidor
            _logger.LogError(ex, "Erro ao processar {Method} {Path}",
                context.Request.Method, context.Request.Path);

            var (status, mensagem) = ex switch
            {
                NotFoundException => (HttpStatusCode.NotFound, ex.Message),
                BusinessException => (HttpStatusCode.BadRequest, ex.Message),
                _ => (HttpStatusCode.InternalServerError,
                      "Ocorreu um erro interno no servidor. Tente novamente mais tarde.")
            };

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            var body = new
            {
                status = (int)status,
                erro = status.ToString(),
                mensagem,
                caminho = context.Request.Path.Value,
                dataHora = DateTime.Now
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(body,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}
