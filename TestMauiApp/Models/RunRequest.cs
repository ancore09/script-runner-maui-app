using System.Text.Json.Serialization;

namespace TestMauiApp.Models;

public class RunRequest
{
    [JsonPropertyName("scriptId")]
    public long ScriptId { get; set; }
    
    [JsonPropertyName("scriptArgs")]
    public string ScriptArgs { get; set; }
}