using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace AvaloniaDesktop.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            this.WhenActivated(disposable =>
            {
                Disposable.Create(() => { }).DisposeWith(disposable);
            });
            InitializeComponent();
            #if DEBUG
            this.AttachDevTools();
            #endif
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
