namespace SimpleDotNet9WebApi.Interfaces;

public interface IExternalApiClient
{
    Task<string> GetExternalDataAsync(string url);
}