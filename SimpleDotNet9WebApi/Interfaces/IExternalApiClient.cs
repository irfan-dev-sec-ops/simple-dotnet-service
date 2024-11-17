using SimpleDotNet9WebApi.Models;

namespace SimpleDotNet9WebApi.Interfaces;

public interface IExternalApiClient
{
    Task<(int StatusCode, string Content)> GetExternalDataAsync(string id);

    Task<(int StatusCode, string Content)> GetExternalDataPostAsync(Student student);
}