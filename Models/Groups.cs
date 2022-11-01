using System.Text.Json.Serialization;

namespace AvaloniaDesktop.Models;
public class Groups
{
    [JsonPropertyName("auth_group_id")]
    public int AuthGroupId { get; set; }
    [JsonPropertyName("id_auth_group_module")]
    public int IdAuthGroupModule { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
