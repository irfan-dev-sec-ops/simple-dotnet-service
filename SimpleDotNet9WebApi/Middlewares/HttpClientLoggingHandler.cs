using System.Diagnostics;
using System.Text;
using log4net;

namespace SimpleDotNet9WebApi.Middlewares;

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
            $"type=CLIENT direction=REQUEST method={request.Method} uri={request.RequestUri}{request.RequestUri?.Query} body={requestBody}");

        var response = await base.SendAsync(request, cancellationToken);

        stopwatch.Stop();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation(
                $"type=CLIENT direction=RESPONSE method={request.Method} uri={request.RequestUri}{request.RequestUri?.Query} body={responseBody} httpStatusCode={(int)response.StatusCode} timeTaken={stopwatch.ElapsedMilliseconds} ms");
        }
        else
        {
            logger.LogWarning(
                $"type=CLIENT direction=RESPONSE method={request.Method} uri={request.RequestUri}{request.RequestUri?.Query} body={responseBody} httpStatusCode={(int)response.StatusCode} timeTaken={stopwatch.ElapsedMilliseconds} ms");
        }

        return response;
    }

    private static string GetHeaders(string currThreadHeaders, System.Net.Http.Headers.HttpRequestHeaders headers)
    {
        var validHeaders = new[]
        {
            "User-Id", "Trace-Id", "Source-Application", "Idempotency-Key", "Content-Type"
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