using AvaloniaDesktop.Models;
using System.Threading.Tasks;

namespace AvaloniaDesktop.Services;

public interface ILoginService
{
    Task<Users> Authentication(string username, string password);
}

