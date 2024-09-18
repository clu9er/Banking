using System.Net;
using System.Text.Json;
using Banking.Application.Models.Results;

namespace Banking.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var result = JsonSerializer.Serialize(Result.Failure(exception.Message));

        return context.Response.WriteAsync(result);
    }
}
