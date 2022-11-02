using System.Text.Json.Serialization;

namespace AvaloniaDesktop.Models;
public class Grants
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
