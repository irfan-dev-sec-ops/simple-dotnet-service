using SimpleDotNet9WebApi.Exceptions;
using SimpleDotNet9WebApi.Interfaces;
using SimpleDotNet9WebApi.Models;
using Newtonsoft.Json;

namespace SimpleDotNet9WebApi.Services;

public class ExternalApiClient(HttpClient httpClient) : IExternalApiClient
{
    public async Task<(int StatusCode, string Content)> GetExternalDataAsync(string id)
    {
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "students/" + id);
            //requestMessage.Headers.Add("api-delay", "100");
            //requestMessage.Headers.Add("api-status-code", "502");
            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) return ((int)response.StatusCode, content);
            var externalErrorMessageJson = await response.Content.ReadFromJsonAsync<ExternalErrorMessage>();

            throw new CommonException(ErrorCode.SimpleCode001, response.StatusCode, ErrorMessage.SimpleCode001,
                externalErrorMessageJson?.ToString());
        }
        catch (HttpRequestException e)
        {
            throw new CommonException(ErrorCode.SimpleCode002, e.StatusCode,
                $"{ErrorMessage.SimpleCode002}{e.Message}");
        }
        catch (CommonException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new CommonException(ErrorCode.SimpleCode003, HttpStatusCode.InternalServerError,
                $"{ErrorMessage.SimpleCode003}{e.Message}");
        }
    }

    public async Task<(int StatusCode, string Content)> GetExternalDataPostAsync(Student student)
    {
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "students");
            requestMessage.Content =
                new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return ((int)response.StatusCode, content);

            var externalErrorMessageJson = await response.Content.ReadFromJsonAsync<ExternalErrorMessage>();
            throw new CommonException(ErrorCode.SimpleCode001, response.StatusCode, ErrorMessage.SimpleCode001,
                externalErrorMessageJson?.ToString());
        }
        catch (HttpRequestException e)
        {
            throw new CommonException(ErrorCode.SimpleCode002, e.StatusCode,
                $"{ErrorMessage.SimpleCode002}{e.Message}");
        }
        catch (CommonException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new CommonException(ErrorCode.SimpleCode003, HttpStatusCode.InternalServerError,
                $"{ErrorMessage.SimpleCode003}{e.Message}");
        }
    }
}