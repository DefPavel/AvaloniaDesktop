using System.Text.Json;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Models.Types;
using DynamicData;

namespace AvaloniaDesktop.Services.Api;

public class QueryService
{
    private static readonly string? ApiUrl = ConfigurationManager.AppSettings["api"];

    #region Десериализация с IObservable

    public static async ValueTask<ObservableCollection<Departments>> JsonDeserializeIObservable(string token, string queryUrl, string httpMethod) 
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = httpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();
        using StreamReader reader = new(responseStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<ObservableCollection<Departments>>(await reader.ReadToEndAsync())
               ?? throw new NullReferenceException();// Возвращаем json информацию которая пришла 
    }
    

    #endregion
    
    #region Обобщение Десериализации С Токеном
    
    
    public static async ValueTask<T> JsonDeserializeWithToken<T>(string token, string queryUrl, string httpMethod) where T : new()
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = httpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();
        using StreamReader reader = new(responseStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<T>(await reader.ReadToEndAsync())
             ?? throw new NullReferenceException();// Возвращаем json информацию которая пришла 
    }

    public static async ValueTask<T> JsonDeserializeWithObjectAndParam<T>(string token, string queryUrl, string httpMethod, T obj)
    {
#pragma warning disable SYSLIB0014 // Тип или член устарел
        var req = (HttpWebRequest)WebRequest.Create(ApiUrl + queryUrl);     // Создаём запрос
#pragma warning restore SYSLIB0014 // Тип или член устарел
        req.Method = httpMethod;                                                        // Выбираем метод запроса
        req.Headers.Add("auth-token", token);
        req.Accept = "application/json";

        await using (StreamWriter streamWriter = new(req.GetRequestStream()))
        {
            req.ContentType = "application/json";
            var json = JsonSerializer.Serialize(obj);
            await streamWriter.WriteAsync(json);
            // Записывает тело
            streamWriter.Close();
        }

        using var response = await req.GetResponseAsync();

        await using var responseStream = response.GetResponseStream();
        using StreamReader reader = new(responseStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<T>(await reader.ReadToEndAsync())
             ?? throw new NullReferenceException();    // Возвращаем json информацию которая пришла 
    }
    #endregion

}

