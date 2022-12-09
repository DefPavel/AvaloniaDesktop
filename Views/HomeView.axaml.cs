using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;

namespace AvaloniaDesktop.Views
{
    public partial class HomeView : ReactiveUserControl<HomeViewModel>
    {
        public HomeView()
        {
            this.WhenActivated(disposables => {
                Disposable.Create(() => { }).DisposeWith(disposables);
            });
            InitializeComponent();
        }
    }
}
