using System.Diagnostics;
using System.Text;
using log4net;
using Newtonsoft.Json;

namespace SimpleDotNet9WebApi.Middleware;

public class HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var requestHeaders = GetHeaders(LogicalThreadContext.Properties["RequestHeaders"] as string ?? "",
            request.Headers);
        LogicalThreadContext.Properties["RequestHeaders"] = requestHeaders;

        var requestBody = request.Content == null ? "" : await request.Content.ReadAsStringAsync(cancellationToken);

        logger.LogInformation(
            "type=CLIENT direction=REQUEST method={Method} uri={Uri} body={Body}", request.Method, request.RequestUri,
            requestBody);

        var response = await base.SendAsync(request, cancellationToken);

        stopwatch.Stop();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        responseBody = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(responseBody),
            new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });
        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation(
                "type=CLIENT direction=RESPONSE method={Method} uri={Uri} body={Body} httpStatusCode={StatusCode} timeTaken={TimeTaken} ms",
                request.Method, request.RequestUri, responseBody, (int)response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        else
        {
            logger.LogWarning(
                "type=CLIENT direction=RESPONSE method={Method} uri={Uri} body={Body} httpStatusCode={StatusCode} timeTaken={TimeTaken} ms",
                request.Method, request.RequestUri, responseBody, (int)response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }

        return response;
    }

    private static string GetHeaders(string currThreadHeaders, System.Net.Http.Headers.HttpRequestHeaders headers)
    {
        var validHeaders = new[]
        {
            "User-Id", "Trace-Id", "Source-Application", "Idempotency-Key", "Content-Type",
            "api-status-code", "api-delay"
        };
        var headerBuilder = new StringBuilder(currThreadHeaders + " ");
        foreach (var header in headers)
        {
            if (validHeaders.Contains(header.Key))
            {
                headerBuilder.Append($"{header.Key.Replace("-", "_")}={string.Join(",", header.Value)} ");
            }
        }

        return headerBuilder.ToString();
    }
}