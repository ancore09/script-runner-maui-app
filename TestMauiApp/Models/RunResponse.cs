using System.Text.Json.Serialization;

namespace TestMauiApp.Models;

public class RunResponse
{
    [JsonPropertyName("standardOutput")]
    public string StandardOutput { get; set; }
    [JsonPropertyName("standardError")]
    public string StandardError { get; set; }
    public string Error { get; set; }
}