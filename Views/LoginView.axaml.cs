using Avalonia.ReactiveUI;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;

namespace AvaloniaDesktop.Views
{
    public partial class LoginView : ReactiveUserControl<LoginViewModel>
    {
        public LoginView()
        {
            this.WhenActivated(disposables => {

            });

            InitializeComponent();
        }
    }
}
