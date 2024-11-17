using Newtonsoft.Json;

namespace SimpleDotNet9WebApi.Models;

public class Student(int id, string name, int age, string email)
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = name;

    [JsonProperty(PropertyName = "age")]
    public int Age { get; set; } = age;

    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; } = id;

    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; } = email;
}