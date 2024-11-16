using SimpleDotNet9WebApi.Interfaces;

namespace SimpleDotNet9WebApi.Services;

public class ExternalApiClient(HttpClient httpClient) : IExternalApiClient
{
public async Task<string> GetExternalDataAsync(string url)
{
    try
    {
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    catch (HttpRequestException e)
    {
        // Handle specific HTTP request exceptions
        // Log the exception or return a custom error message
        return $"Request error: {e.Message}";
    }
    catch (Exception e)
    {
        // Handle other exceptions
        // Log the exception or return a custom error message
        return $"An error occurred: {e.Message}";
    }
}
}