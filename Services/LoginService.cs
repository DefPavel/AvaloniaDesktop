using AvaloniaDesktop.Models;
using System.Threading.Tasks;
using AvaloniaDesktop.Helpers;
using AvaloniaDesktop.Services.Api;

namespace AvaloniaDesktop.Services;

public class LoginService : ILoginService
{
    // Ключ шифрования
    private const string KeyEncrypt = "8UHjPgXZzXDgkhqV2QCnooyJyxUzfJrO";
    public async Task<Users> Authentication(string username, string password)
    {
        // Кодирования пароля
        var encryptedPass = CustomAes256.Encrypt(password, KeyEncrypt);

        var payloadUser = new Users
        {
            UserName = username,
            Password = encryptedPass,
        };
        // Отправляем запрос
        return await QueryService.JsonDeserializeWithObjectAndParam(token: "", "/auth", "POST", payloadUser);
    }
}

