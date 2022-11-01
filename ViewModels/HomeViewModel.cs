using AvaloniaDesktop.Models;
using ReactiveUI;
using System.Reactive.Disposables;

namespace AvaloniaDesktop.ViewModels;

public sealed class HomeViewModel : ViewModelBase, IScreen, IRoutableViewModel
{
    private readonly Users _account;
    public string UrlPathSegment => nameof(HomeViewModel);
    public IScreen HostScreen { get; }
    public RoutingState Router { get; } = new RoutingState();


    public HomeViewModel(IScreen hostScreen, Users account)
    {
        HostScreen = hostScreen;
        _account = account;

        this.WhenActivated((CompositeDisposable disposables) =>
        {

        });
    }
}

