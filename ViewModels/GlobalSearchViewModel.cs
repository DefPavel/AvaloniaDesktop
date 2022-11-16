using System.Reactive;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Services;
using ReactiveUI;
using Splat;

namespace AvaloniaDesktop.ViewModels;

public sealed class GlobalSearchViewModel :  ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment => nameof(GlobalSearchViewModel);
    public IScreen HostScreen { get; }
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack { get; }
    
    
    public GlobalSearchViewModel(IScreen hostScreen , Users account) :
        this(hostScreen,
            account,
            Locator.Current.GetService<ISearchService>()) { }

    private GlobalSearchViewModel(IScreen hostScreen, Users account, ISearchService getService)
    {
        HostScreen = hostScreen;

        GoBack = HostScreen.Router.NavigateBack;
    }
}