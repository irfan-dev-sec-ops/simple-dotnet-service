using System.Diagnostics;
using log4net;
using Newtonsoft.Json;

namespace SimpleDotNet9WebApi.Middleware;

public class RequestResponseLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        LogicalThreadContext.Properties["RequestHeaders"] = GetHeaders(context.Request.Headers);

        var request = context.Request;
        context.Request.EnableBuffering();

        request.Body.Seek(0, SeekOrigin.Begin);
        var requestBody = await new StreamReader(request.Body, Encoding.UTF8).ReadToEndAsync();
        requestBody = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(requestBody),
            new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });
        request.Body.Seek(0, SeekOrigin.Begin);

        logger.LogInformation(
            "type=SERVER direction=REQUEST method={Method} uri={Uri} body={Body}", request.Method,
            $"{request.Path}{request.QueryString}", requestBody);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next.Invoke(context);

        stopwatch.Stop();
        responseBody.Seek(0, SeekOrigin.Begin);
        var content = await new StreamReader(responseBody, Encoding.UTF8).ReadToEndAsync();
        content = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(content), new JsonSerializerSettings
        {
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
        });
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);

        if (context.Response.StatusCode < 400)
        {
            logger.LogInformation(
                "type=SERVER direction=RESPONSE method={Method} uri={Uri} body={Body} httpStatusCode={StatusCode} timeTaken={TimeTaken} ms",
                request.Method, $"{request.Path}{request.QueryString}", content, context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        else
        {
            logger.LogWarning(
                "type=SERVER direction=RESPONSE method={Method} uri={Uri} body={Body} httpStatusCode={StatusCode} timeTaken={TimeTaken} ms",
                request.Method, $"{request.Path}{request.QueryString}", content, context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }

        context.Response.Body = originalBodyStream;
        LogicalThreadContext.Properties.Remove("RequestHeaders");
    }

    private string GetHeaders(IHeaderDictionary headers)
    {
        var validHeaders = new[]
        {
            "User-Id", "Trace-Id", "Source-Application", "Idempotency-Key", "Content-Type"
        };
        return string.Join(" ",
            headers.Where(header => validHeaders.Contains(header.Key))
                .Select(header => $"{header.Key.Replace("-", "_")}={header.Value}"));
    }
}