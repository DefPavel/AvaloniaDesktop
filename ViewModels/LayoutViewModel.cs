using System.Reactive;
using System.Reactive.Disposables;
using AvaloniaDesktop.Models;
using ReactiveUI;

namespace AvaloniaDesktop.ViewModels;

public sealed class LayoutViewModel : ViewModelBase, IScreen, IRoutableViewModel
{
    public string UrlPathSegment => nameof(LayoutViewModel);
    public IScreen HostScreen { get; }
    public RoutingState Router { get; } = new();
    public ReactiveCommand<Unit, Unit> Logout { get; }
    
    public LayoutViewModel(IScreen screen, Users account)
    {
        HostScreen = screen;
        Logout = ReactiveCommand.Create(() => { HostScreen.Router.NavigateAndReset.Execute(new LoginViewModel(HostScreen)); });
        Router.Navigate.Execute(new HomeViewModel(HostScreen, account));

        this.WhenActivated((CompositeDisposable disposables) =>
        {

        });
    }
}