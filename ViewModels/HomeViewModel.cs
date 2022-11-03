using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaDesktop.Models;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Runtime.InteropServices;
using AvaloniaDesktop.Services;
using DynamicData;
using Splat;

namespace AvaloniaDesktop.ViewModels;

public sealed class HomeViewModel : ReactiveObject, IActivatableViewModel, IRoutableViewModel
{

    #region Свойства

    private ObservableCollection<Departments> _itemsDepartmentsList;
    public ObservableCollection<Departments> ItemsDepartmentsList => _itemsDepartmentsList;

    public ViewModelActivator Activator { get; } = new();

    public string UrlPathSegment => nameof(HomeViewModel);
    
    private readonly IHomeService? _homeService;
    
    private readonly Users _account;
    public IScreen HostScreen { get; }
    

    #endregion
    public HomeViewModel(IScreen hostScreen , Users account) :
        this(hostScreen,
            account,
            Locator.Current.GetService<IHomeService>()) { }
    public HomeViewModel(IScreen hostScreen, Users account, IHomeService? homeService)
    {
        HostScreen = hostScreen;
        _account = account;
        _homeService = homeService;
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            // LoadedDepartments();
        });
    }

    private async void LoadedDepartments()
    {
        var departments = await _homeService!.GetTreeDepartments(_account);

        foreach (var item in departments)
        {
            ItemsDepartmentsList.Add(item);
        }
    }

    
}

