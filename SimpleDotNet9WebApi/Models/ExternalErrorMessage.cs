namespace SimpleDotNet9WebApi.Models;

public abstract class ExternalErrorMessage(int Code, string message) {
    public int Code { get; set; }
    private string Message { get; set; } = message;

    public override string ToString() {
        return $"{{\"Code\":{Code},\"Message\":\"{Message}\"}}";
    }
}