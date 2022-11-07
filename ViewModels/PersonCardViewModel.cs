using System;
using System.Reactive.Disposables;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Services;
using ReactiveUI;
using Splat;

namespace AvaloniaDesktop.ViewModels;

public sealed class PersonCardViewModel :  ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(PersonCardViewModel);
    
    public IScreen HostScreen { get; }

    public PersonCardViewModel(IScreen hostScreen , Users account) :
        this(hostScreen,
            account,
            Locator.Current.GetService<ICardService>()) { }

    public PersonCardViewModel(IScreen hostScreen, Users account, ICardService? getService)
    {
       this.WhenActivated((CompositeDisposable disposables) =>
       {
           GC.Collect();
       });
    }
}