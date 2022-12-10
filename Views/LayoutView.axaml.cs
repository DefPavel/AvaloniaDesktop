using System.Reactive.Disposables;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;

namespace AvaloniaDesktop.Views;
public partial class LayoutView : ReactiveUserControl<LayoutViewModel>
{
    public LayoutView()
    {
        this.WhenActivated(disposables => {
            Disposable.Create(() => { }).DisposeWith(disposables);
        });

        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}