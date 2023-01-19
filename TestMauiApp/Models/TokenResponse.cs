using System.Text.Json.Serialization;

namespace TestMauiApp.Models;

public class TokenResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
    public string Error { get; set; }
}