using System.Text.Json.Serialization;

namespace TestMauiApp.Models;

public class User
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("username")]

    public string Username { get; set; }
}