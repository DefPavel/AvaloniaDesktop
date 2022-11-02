using System;
using System.Collections.ObjectModel;
using AvaloniaDesktop.Models;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using AvaloniaDesktop.Services;
using DynamicData;
using Splat;

namespace AvaloniaDesktop.ViewModels;

public sealed class HomeViewModel : ReactiveObject, IActivatableViewModel, IRoutableViewModel
{

    #region Свойства

    private ReadOnlyObservableCollection<Departments> _itemsDepartmentsList;
    public ReadOnlyObservableCollection<Departments> ItemsDepartmentsList => _itemsDepartmentsList;

    private readonly IHomeSerivce? _homeService;
    
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    public string UrlPathSegment { get; } = nameof(HomeViewModel);
    public IScreen HostScreen { get; }

    #endregion
    public HomeViewModel(IScreen hostScreen , Users account) :
        this(hostScreen,
            account,
            Locator.Current.GetService<IHomeSerivce>()) { }
    public HomeViewModel(IScreen hostScreen, Users account, IHomeSerivce? homeService)
    {
        HostScreen = hostScreen;
        _homeService = homeService;
        
        

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            // _itemsDepartmentsList = _homeService!.GetTreeDepartments(account);
            _homeService!
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                // We .Bind() and now our mutable Items collection 
                // contains the new items and the GUI gets refreshed.
                .Bind(out _itemsDepartmentsList)
                .Subscribe();
        });
    }

    
}

