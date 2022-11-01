using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AvaloniaDesktop.ViewModels;
public class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    protected ObservableAsPropertyHelper<bool> isBusy;
    public bool IsBusy { get { return isBusy.Value; } }
}

