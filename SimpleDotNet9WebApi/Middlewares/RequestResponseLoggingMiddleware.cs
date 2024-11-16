using System.Diagnostics;
using System.Text;
using log4net;
using Newtonsoft.Json;

namespace SimpleDotNet9WebApi.Middlewares
{
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
            requestBody = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(requestBody));
            request.Body.Seek(0, SeekOrigin.Begin);

            logger.LogInformation(
                $"type=SERVER direction=REQUEST method={request.Method} uri={request.Path}{request.QueryString} body={requestBody}");

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next.Invoke(context);

            stopwatch.Stop();
            responseBody.Seek(0, SeekOrigin.Begin);
            var content = await new StreamReader(responseBody, Encoding.UTF8).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);

            if (context.Response.StatusCode < 400)
            {
                logger.LogInformation(
                    $"type=SERVER direction=RESPONSE method={request.Method} uri={request.Path}{request.QueryString} body={content} httpStatusCode={context.Response.StatusCode} timeTaken={stopwatch.ElapsedMilliseconds} ms");
            }
            else
            {
                logger.LogWarning(
                    $"type=SERVER direction=RESPONSE method={request.Method} uri={request.Path}{request.QueryString} body={content} httpStatusCode={context.Response.StatusCode} timeTaken={stopwatch.ElapsedMilliseconds} ms");
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
}