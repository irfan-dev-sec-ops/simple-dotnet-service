namespace SimpleDotNet9WebApi.Models;

[Serializable]
public class Error(ErrorCode errorCode, string message, string? externalMessage = null)
{
    public ErrorCode ErrorCode { get; set; } = errorCode;
    public string Message { get; set; } = message;
    public string? ExternalMessage { get; } = externalMessage;

    public override string ToString()
    {
        return $"{{\"ErrorCode\":{ErrorCode},\"Message\":\"{Message}\",\"ExternalMessage\":\"{ExternalMessage}\"}}";
    }
}