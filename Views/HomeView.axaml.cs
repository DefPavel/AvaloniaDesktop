using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using AvaloniaDesktop.Models.Types;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;

namespace AvaloniaDesktop.Views
{
    public partial class HomeView : ReactiveUserControl<HomeViewModel>
    {
        
        //public AutoCompleteBox autoTypePosition => this.FindControl<AutoCompleteBox>("AutoTypePosition");
        public HomeView()
        {
            this.WhenActivated(disposables => {
                Disposable.Create(() => { }).DisposeWith(disposables);
            });

            
            /*this.Bind(ViewModel, vm => vm.PopulateAsync(autoTypePosition.Text, new CancellationToken()),
                v => v.autoTypePosition.AsyncPopulator).Dispose();
            */
            InitializeComponent();
        }
    }
}
