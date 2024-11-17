using System.Text.Json.Serialization;
using SimpleDotNet9WebApi.Interfaces;
using SimpleDotNet9WebApi.Middleware;
using SimpleDotNet9WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddFilter("Microsoft", LogLevel.Information)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("Npgsql", LogLevel.Warning);
});
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient<IExternalApiClient, ExternalApiClient>(client =>
    {
        client.BaseAddress = new Uri("http://localhost:9080/students-api/v1/");
    })
    .AddHttpMessageHandler<HttpClientLoggingHandler>();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<HttpClientLoggingHandler>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();