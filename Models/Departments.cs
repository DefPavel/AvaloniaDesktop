using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using AvaloniaDesktop.Models.Types;

namespace AvaloniaDesktop.Models;

public sealed class Departments : TypeDepartments
{
    [JsonPropertyName("id")] public int Id { get; set; } = 0;

    [JsonPropertyName("name_depart")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("child")] public IEnumerable<Departments> Child { get; set; } = Enumerable.Empty<Departments>().ToArray();

    [JsonPropertyName("short")] public string Short { get; set; } = string.Empty;

    [JsonPropertyName("parentId")] public int ParentId { get; set; }

    [JsonPropertyName("phone")] public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("root")] public string RootTree { get; set; } = string.Empty;
}