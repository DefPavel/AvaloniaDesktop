using System.Reactive.Disposables;
using Avalonia.Markup.Xaml;
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
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}