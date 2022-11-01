using AvaloniaDesktop.Services;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AvaloniaDesktop.ViewModels;

public sealed class LoginViewModel : ViewModelBase, IRoutableViewModel
{

    #region Свойства

    private readonly ILoginService _loginService;
    public string UrlPathSegment { get; } = "login";
    public IScreen HostScreen { get; }


    private string? _username;
    public string? Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    private string? _password;
    private IScreen screen;
    private ILoginService loginService;

    public string? Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    #endregion
    public LoginViewModel(IScreen screen) :
       this(screen, Locator.Current.GetService<ILoginService>())
    {

    }
    public LoginViewModel(IScreen screen, ILoginService loginService)
    {
        this.screen = screen;
        this.loginService = loginService;

        // Проверка доступности отправки json
        var canLogin = this.WhenAnyValue(x => x.Username, x => x.Password,
        (userName, password) =>
            !string.IsNullOrWhiteSpace(userName) &&
            !string.IsNullOrWhiteSpace(password));

        // Привязываем команду типа Task
        Login = ReactiveCommand.CreateFromTask(LoginAsync, canLogin);

        Login.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            // Added here just for testing
            GC.Collect();
        });
    }

    #region Команды
    public ReactiveCommand<Unit, Unit> Login { get; }

    public ReactiveCommand<Unit, Unit> Exit { get; }

    #endregion

    #region Логика

    private async Task LoginAsync()
    {
        var result = await _loginService.Authentication(Username!, Password!);
    }

    #endregion

}

