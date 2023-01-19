using System.Text.Json.Serialization;

namespace TestMauiBackend.Models;

public class RunRequest
{
    [JsonPropertyName("scriptId")]
    public long ScriptId { get; set; }
    
    [JsonPropertyName("scriptArgs")]
    public string ScriptArgs { get; set; }
}