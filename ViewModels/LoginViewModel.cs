﻿using AvaloniaDesktop.Models;
using AvaloniaDesktop.Services;
using AvaloniaDesktop.Utils;
using ReactiveUI;
using Splat;
using System;
using System.IO;
using System.Net;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AvaloniaDesktop.ViewModels;

public sealed class LoginViewModel : ViewModelBase, IRoutableViewModel
{
    #region Свойства

    private readonly ILoginService? _loginService;
    private readonly IApplicationInfo? _applicationInfo;
    public string UrlPathSegment => "Login";
    public IScreen HostScreen { get; }

    private string? _username;
    public string? Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);

    }
    private string? _password;
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
    
    private bool _isRememberMe;
    public bool IsRememberMe
    {
        get => _isRememberMe;
        set => this.RaiseAndSetIfChanged(ref _isRememberMe, value);
    }
    
    public string Version => $"Версия приложения: {_applicationInfo?.FileVersion}";
    #endregion

    public LoginViewModel(IScreen screen) :
       this(screen, 
           Locator.Current.GetService<ILoginService>(),
           Locator.Current.GetService<IApplicationInfo>()){ }
    public LoginViewModel(IScreen screen, ILoginService? loginService, IApplicationInfo? applicationInfo)
    {
        HostScreen = screen;
        _loginService = loginService;
        _applicationInfo = applicationInfo;


        #if DEBUG
        Username = "1978";
        Password = "root";
        #endif
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
    #endregion

    #region Логика

    private async Task LoginAsync()
    {
        Users account;
        try
        {
            account = await _loginService!.Authentication(Username!, Password!);
            await HostScreen.Router.NavigateAndReset.Execute(new LayoutViewModel(HostScreen, account));

        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());
                    account = JsonSerializer.Deserialize<Users>(await reader.ReadToEndAsync()) ?? throw new InvalidOperationException();
                    ErrorMessage = account.Error;
                }
            }
            else
            {
               ErrorMessage = "Не удалось получить данные с API!";
            }
        }
       
    }

    #endregion

}

