using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;

namespace AvaloniaDesktop.Views;

public partial class SearchView : ReactiveUserControl<GlobalSearchViewModel>
{
    public SearchView()
    {
        this.WhenActivated(disposables =>
        {
            Disposable.Create(() => { }).DisposeWith(disposables);
        });
        InitializeComponent();
    }
}