using ReactiveUI;

namespace AvaloniaDesktop.ViewModels;

public class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    protected ObservableAsPropertyHelper<bool> isBusy;
    public bool IsBusy { get { return isBusy.Value; } }
}

