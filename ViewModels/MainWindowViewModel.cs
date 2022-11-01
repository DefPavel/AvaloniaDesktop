using ReactiveUI;
using System.Reactive.Disposables;

namespace AvaloniaDesktop.ViewModels;
public sealed class MainWindowViewModel : ReactiveObject, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();
    public RoutingState Router { get; } = new RoutingState();

    public MainWindowViewModel()
    {
        var loginViewModel = new LoginViewModel(this);
        Router.Navigate.Execute(loginViewModel);

        this.WhenActivated((CompositeDisposable disposables) =>
        {

        });
       
    }
}

