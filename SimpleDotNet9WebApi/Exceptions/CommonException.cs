using SimpleDotNet9WebApi.Models;

namespace SimpleDotNet9WebApi.Exceptions;

public class CommonException(
    ErrorCode errorCode,
    HttpStatusCode? httpStatusCode,
    string message,
    string? externalMessage = null)
    : Exception(message)
{
    public ErrorCode ErrorCode { get; } = errorCode;
    public HttpStatusCode? HttpStatusCode { get; } = httpStatusCode;
    public string? ExternalMessage { get; } = externalMessage;
}