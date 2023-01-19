using System.Text.Json.Serialization;

namespace TestMauiApp.Models;

public record Script(
    [property: JsonPropertyName("id")]
    long Id,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("args")]
    string Args
    );