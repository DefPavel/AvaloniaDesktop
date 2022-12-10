using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;

namespace AvaloniaDesktop.Views
{
    public partial class LoginView : ReactiveUserControl<LoginViewModel>
    {
        public ProgressBar Progress => this.FindControl<ProgressBar>("ProgressBar");

        public LoginView()
        {
            this.WhenActivated(disposables =>
            {
                Disposable.Create(() => { }).DisposeWith(disposables);

                this.WhenAnyObservable(x => x.ViewModel!.Login.IsExecuting)
                    .BindTo(this, x => x.Progress.IsVisible)
                    .DisposeWith(disposables);

            });
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
    }
}
