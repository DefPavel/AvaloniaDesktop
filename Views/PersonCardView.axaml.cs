﻿using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using AvaloniaDesktop.ViewModels;
using ReactiveUI;

namespace AvaloniaDesktop.Views;

public partial class PersonCardView : ReactiveUserControl<PersonCardViewModel>
{
    public PersonCardView()
    {
        this.WhenActivated(disposables =>
        {
            Disposable.Create(() => { }).DisposeWith(disposables);

                    
        });
        InitializeComponent();
    }
}