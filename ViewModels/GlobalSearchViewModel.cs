using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AvaloniaDesktop.ViewModels;

public sealed class GlobalSearchViewModel :  ViewModelBase, IRoutableViewModel
{
    #region Свойства
    public string? UrlPathSegment => nameof(GlobalSearchViewModel);
    public IScreen HostScreen { get; }

    private readonly ISearchService? _searchService;
    [Reactive] public string? SearchQuery { get; set; }
    
    [Reactive] public ObservableCollection<Departments>? DepartmentsList { get; set; } = new();
    
    [Reactive] public ObservableCollection<Persons>? PersonsList { get; set; } = new();

    #endregion


    #region Команды
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public ReactiveCommand<Unit, Unit> SearchPerson { get; }
    
    #endregion

    #region Логика

    private async Task GoToHome(Users account)
    {
        await HostScreen.Router.NavigateAndReset.Execute(new LayoutViewModel(HostScreen, account));
    }

    private async Task SearchApiPerson(Users account)
    {
        try
        {
            PersonsList = await _searchService!.GetPersonsBySearch(account, SearchQuery!);
        }
        // Ошибка токена
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Предупреждение",
                    "Вы не проявляли активности в программе более 30 минут!");
            await messageBoxStandardWindow.Show();

            HostScreen.Router.NavigateAndReset.Execute(new LoginViewModel(HostScreen));
        }
        // Ошибка с сервера
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());
                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Ошибочка", await reader.ReadToEndAsync());
                    await messageBoxStandardWindow.Show();
                }
            }
        }
        // Что-то опасное
        catch (Exception ex)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибочка", ex.Message);
            await messageBoxStandardWindow.Show();
        }
       
    }

    #endregion
   

    public GlobalSearchViewModel(IScreen hostScreen , Users account) :
        this(hostScreen,
            account,
            Locator.Current.GetService<ISearchService>()) { }

    public GlobalSearchViewModel(IScreen hostScreen, Users account, ISearchService? getService)
    {
        HostScreen = hostScreen;
        _searchService = getService;
        GoBack = ReactiveCommand.CreateFromTask(async _ => await GoToHome(account));
        
        var canSearch = this.WhenAny(x => x.SearchQuery, x => 
            !string.IsNullOrWhiteSpace(x.Value));

        SearchPerson = ReactiveCommand.CreateFromTask(async _ => await SearchApiPerson(account), canSearch);
    }
}