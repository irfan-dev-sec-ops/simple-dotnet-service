using Newtonsoft.Json;

namespace SimpleDotNet9WebApi.Models;

public class ExternalErrorMessage(int Code, string message) {
    [JsonProperty(PropertyName = "code")]
    public int Code { get; set; }
    [JsonProperty(PropertyName = "message")]

    public string Message { get; set; } = message;

    public override string ToString() {
        return $"{{\"Code\":{Code},\"Message\":\"{Message}\"}}";
    }
}