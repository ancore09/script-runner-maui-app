using System.Text.Json.Serialization;

namespace TestMauiApp.Models;

public class LoginResponse
{
    [JsonPropertyName("user")]
    public User User { get; set; }
    [JsonPropertyName("tokens")]
    public TokenResponse Tokens { get; set; }
    public string Error { get; set; }
}