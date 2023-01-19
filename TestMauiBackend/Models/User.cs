using System.Text.Json.Serialization;

namespace TestMauiBackend.Models;

public class User
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
}