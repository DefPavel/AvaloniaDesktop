using System.Text.Json.Serialization;

namespace AvaloniaDesktop.Models.Types;

public sealed class TypePosition
{
    [JsonPropertyName("id")] public int Id { get; set; }
    //[JsonPropertyName("id_type")] public int IdType { get; set; }
    [JsonPropertyName("name_position")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("is_ped")] public bool IsPed { get; set; } = false;
    [JsonPropertyName("holiday_limit")] public short LimitHoliday { get; set; } = 0;
    [JsonPropertyName("name_genitive")] public string NameGenitive { get; set; } = string.Empty;
    [JsonPropertyName("priority")] public int Priority { get; set; } = 0;
}