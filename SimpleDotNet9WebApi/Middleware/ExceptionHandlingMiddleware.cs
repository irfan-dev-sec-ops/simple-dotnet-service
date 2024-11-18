using System.Text.Json;
using System.Text.Json.Serialization;
using SimpleDotNet9WebApi.Exceptions;
using SimpleDotNet9WebApi.Models;

namespace SimpleDotNet9WebApi.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CommonException ex)
        {
            await HandleExceptionAsync(context, ex, ex.ErrorCode, ex.HttpStatusCode, ex.ExternalMessage);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, ErrorCode.SimpleCode003, HttpStatusCode.InternalServerError);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception,
        ErrorCode errorCode, HttpStatusCode? httpStatusCode, string? externalMessage = null)
    {
        var error = new Error(errorCode, exception.Message, externalMessage);
        var result = JsonSerializer.Serialize(error, JsonSerializerOptions);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode =
            httpStatusCode.HasValue ? (int)httpStatusCode : (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }
}