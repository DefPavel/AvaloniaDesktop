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

    private readonly ISearchService? _searchService;
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack { get; }

    public GlobalSearchViewModel(IScreen hostScreen , Users account) :
        this(hostScreen,
            account,
            Locator.Current.GetService<ISearchService>()) { }

    public GlobalSearchViewModel(IScreen hostScreen, Users account, ISearchService? getService)
    {
        HostScreen = hostScreen;
        _searchService = getService;
        GoBack = HostScreen.Router.NavigateBack;
    }
}