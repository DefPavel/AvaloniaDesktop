using AvaloniaDesktop.Services;
using AvaloniaDesktop.Utils;
using Splat;
using System.Reflection;

namespace AvaloniaDesktop;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        // регистрация сервисов
        services.Register<ILoginService>(() => new LoginService());
        // информация о приложении
        services.RegisterLazySingleton<IApplicationInfo>(() => new ApplicationInfo(Assembly.GetExecutingAssembly()));
        

    }
}

