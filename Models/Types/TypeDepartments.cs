using System.Text.Json.Serialization;

namespace AvaloniaDesktop.Models.Types;

public class TypeDepartments
{
    [JsonPropertyName("id_type")] public int IdType { get; set; }
    [JsonPropertyName("type")]  public string TypeName { get; set; } = string.Empty;
}