using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaDesktop.ViewModels;
using AvaloniaDesktop.Views;
using ReactiveUI;
using Splat;

namespace AvaloniaDesktop
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Locator.CurrentMutable.Register<IViewFor<GlobalSearchViewModel>>(() => new SearchView());
                Locator.CurrentMutable.Register<IViewFor<PersonCardViewModel>>(() => new PersonCardView());
                Locator.CurrentMutable.Register<IViewFor<HomeViewModel>>(() => new HomeView());
                Locator.CurrentMutable.Register<IViewFor<LoginViewModel>>(() => new LoginView());
                Locator.CurrentMutable.Register<IViewFor<LayoutViewModel>>(() => new LayoutView());
                Bootstrapper.Register(Locator.CurrentMutable, Locator.Current);

                var viewModel = new MainWindowViewModel();

                desktop.MainWindow = new MainWindow()
                {
                    DataContext = viewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
