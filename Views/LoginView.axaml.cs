using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;

namespace AvaloniaDesktop.Views
{
    public partial class LoginView : ReactiveUserControl<LoginViewModel>
    {
        public LoginView()
        {
            this.WhenActivated(disposables =>
            {
                Disposable.Create(() => { }).DisposeWith(disposables);
                
                this.WhenAnyObservable(x => x.ViewModel!.Login.IsExecuting)
                    .BindTo(this, x => x.ProgressBar.IsVisible)
                    .DisposeWith(disposables);
                    
            });

            InitializeComponent();
        }
    }
}
