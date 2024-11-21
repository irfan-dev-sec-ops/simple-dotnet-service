namespace SimpleDotNet9WebApi.Models;

[Serializable]
public class Error(ErrorCode errorCode, string message, string? externalMessage = null)
{
    public ErrorCode ErrorCode { get; set; } = errorCode;
    public string ErrorMessage { get; set; } = message;
    public string? BackendError { get; } = externalMessage;

    public override string ToString()
    {
        return $"{{\"ErrorCode\":{ErrorCode.GetTypeCode().ToString()},\"ErrorMessage\":\"{ErrorMessage}\",\"BackendError\":\"{BackendError}\"}}";
    }
}