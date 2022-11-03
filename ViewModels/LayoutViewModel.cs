using System.Reactive;
using System.Reactive.Disposables;
using AvaloniaDesktop.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AvaloniaDesktop.ViewModels;

public sealed class LayoutViewModel : ViewModelBase, IScreen, IRoutableViewModel
{
    public string UrlPathSegment => nameof(LayoutViewModel);
    public IScreen HostScreen { get; }
    public RoutingState Router { get; } = new();
    [Reactive] public ReactiveCommand<Unit, Unit> Logout { get; set; }
    [Reactive] public Users Users { get; set; }

    public LayoutViewModel(IScreen screen, Users account)
    {
        HostScreen = screen;
        Users = account;
        
        Logout = ReactiveCommand.Create(() => { HostScreen.Router.NavigateAndReset.Execute(new LoginViewModel(HostScreen)); });
        Router.Navigate.Execute(new HomeViewModel(HostScreen, Users));

        this.WhenActivated((CompositeDisposable disposables) =>
        {

        });
    }
}