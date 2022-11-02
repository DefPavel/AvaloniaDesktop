using System.Text.Json.Serialization;

namespace AvaloniaDesktop.Models;
public class Users
{
    [JsonPropertyName("id")]
    public int Id { get; set; } // Id пользователя

    [JsonPropertyName("login")]
    public string UserName { get; set; } = string.Empty; // Логин

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty; // Пароль

    [JsonPropertyName("auth_token")]
    public string Token { get; set; } = string.Empty; // Токен для middleware

    [JsonPropertyName("id_module")]
    public ModulesProject IdModules { get; set; } // Модуль программы

    [JsonPropertyName("groups")]
    public Groups[]? GroupName { get; set; } // К какой группе пользователей относиться пользователь

    [JsonPropertyName("grants")]
    public Grants[]? Grants { get; set; }// Права доступа пользователя

    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;

    public string FullName => $"{UserName}";
}

