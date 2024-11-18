using Newtonsoft.Json;

namespace SimpleDotNet9WebApi.Models;

public class Student(int id, string name, int age, string email)
{
    [JsonProperty(PropertyName = "name")]
    public required string Name { get; set; } = name;

    [JsonProperty(PropertyName = "age")]
    public required int Age { get; set; } = age;

    [JsonProperty(PropertyName = "id")]
    public required int Id { get; set; } = id;

    [JsonProperty(PropertyName = "email")]
    public required string Email { get; set; } = email;
}