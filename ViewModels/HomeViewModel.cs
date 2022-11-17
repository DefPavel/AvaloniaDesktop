using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using AvaloniaDesktop.Models;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AvaloniaDesktop.Models.Types;
using AvaloniaDesktop.Services;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AvaloniaDesktop.ViewModels;

public sealed class HomeViewModel : ViewModelBase, IRoutableViewModel
{
    #region Свойства
    
    // public RoutingState Router { get; } = new();
    [Reactive] public ObservableCollection<Departments> DepartmentsList { get; set; } = new();
    [Reactive] public Departments SelectedDepartments { get; set; } = new();

    #region Расчет количества сотрудников
    // public int CountAllPerson { get; set; }
    [Reactive] public int CountIsPedPerson { get; set; }
    [Reactive] public int CountNotIsPedPerson { get; set; }
    [Reactive] public int CountIsPluralismInnerIsPed { get; set; }
    [Reactive] public int CountIsPluralismInnerNotIsPed { get; set; }
    [Reactive] public int CountIsPluralismOterIsPed { get; set; }
    [Reactive] public int CountIsPluralismOterNotIsPed { get; set; }
    [Reactive] public decimal CountFreeBudget { get; set; }
    [Reactive] public decimal CountFreeNotBudget { get; set; }
    [Reactive] public decimal CountBudget { get; set; }
    [Reactive] public decimal CountNotBudget { get; set; }
    
    #endregion
    [Reactive] public IEnumerable<string>? TypePosition { get; set; }
    [Reactive] public string? FilterName { get; set; }
    [Reactive] public ObservableCollection<Position>? PositionsList { get; set; } = new();
    [Reactive] public Position SelectedPosition { get; set; } = new();
    [Reactive] public Persons SelectedPersons { get; set; } = new();
    
    private SourceList<Persons> _personsSourceList = new();

    private readonly ReadOnlyObservableCollection<Persons> _personsList;
    public ReadOnlyObservableCollection<Persons> PersonsList => _personsList;

    public string UrlPathSegment => nameof(HomeViewModel);
    private readonly IHomeService? _homeService;
    private readonly Users _account;
    public IScreen HostScreen { get; }
    #endregion
    
    #region Команды
    private ReactiveCommand<Unit, Unit> GetAllDepartment { get; }
    public ReactiveCommand<Unit, Unit> GetPersonsByDepartment { get; }
    public ReactiveCommand<Unit, Unit> NavigateToCard { get; }

    #endregion
    
    #region Логика
    // Фильтр по Ф.И.О.
    private static Func<Persons, bool> Filter(string? filterName)
    {
        if (string.IsNullOrEmpty(filterName)) return _ => true;
        return x => x.FullName.ToUpper().Contains(filterName.ToUpper());
    }
    // Загрузка дерева
    private async Task LoadedTreeAsync()
    {
        var departments = await _homeService!
            .GetTreeDepartments(_account);
        DepartmentsList = new ObservableCollection<Departments>(departments);
        
        // получить все должности
        var allPositions = await _homeService!.GetAllPosition(_account);
        TypePosition = allPositions.Select(variable => variable.Name).ToArray();
    }

    private void GoToPersonCard(Users account , Persons persons, Departments departments)
    {
        // Console.Write(account);
        //HostScreen.Router.NavigateAndReset.Execute(new PersonCardViewModel(HostScreen, account, persons, departments));
        HostScreen.Router.Navigate.Execute(new PersonCardViewModel(HostScreen, account, persons, departments));
        // Router.Navigate.Execute(new LoginViewModel(HostScreen));
    }
    private async Task GetPersonByTreeItemAsync()
    {
        // получить список сотрудников выбранного отдела
        var personsByDepartment = await _homeService!
            .GetPersonsByDepartment(_account, SelectedDepartments);
        _personsSourceList.Clear();
        _personsSourceList.AddRange(personsByDepartment);

        SelectedPersons = PersonsList.FirstOrDefault()!;
        
        // получить штатные должности выбранного отдела
        var positionByDepartment = await _homeService!.GetPositionsByDepartment(_account, SelectedDepartments);
        PositionsList = positionByDepartment;

        SelectedPosition = PositionsList.FirstOrDefault()!;
        

        
        // Считаем сколько НПП
        CountIsPedPerson = _personsList.DistinctBy(x => x.Id).Count(x => x.IsPed);
        // Считаем сколько не НПП
        CountNotIsPedPerson = _personsList.DistinctBy(x => x.Id).Count(x => x.IsPed == false);
        // Совместителей Внутренних НПП
        CountIsPluralismInnerIsPed = _personsList.DistinctBy(x => x.Id).Count(x => x.IsPluralismInner && x.IsPed);
        // Совместителей Внутренних не НПП
        CountIsPluralismInnerNotIsPed = _personsList.Count(x => x.IsPluralismInner && x.IsPed == false);
        // Совместителей Внешних НПП
        CountIsPluralismOterIsPed = _personsList.DistinctBy(x => x.Id).Count(x => x.IsPluralismOter && x.IsPed);
        // Совместителей Внешних не НПП
        CountIsPluralismOterNotIsPed = _personsList.DistinctBy(x => x.Id).Count(x => x.IsPluralismOter && x.IsPed == false);
        // Считаем свободных бюджетных ставки
        CountFreeBudget = PositionsList!.Sum(x => x.Free_B);
        // Считаем свободных внебюджетных ставки
        CountFreeNotBudget = PositionsList!.Sum(x => x.Free_NB);
        // итоги бюджетных ставок
        CountBudget = _personsList.Sum(x => x.StavkaBudget);
        // итоги внебюджетных ставок
        CountNotBudget = _personsList.Sum(x => x.StavkaNoBudget);
    }
    #endregion
    
    public HomeViewModel(IScreen hostScreen , Users account) :
        this(hostScreen,
            account,
            Locator.Current.GetService<IHomeService>()) { }
    public HomeViewModel(IScreen hostScreen, Users account, IHomeService? homeService)
    {
        HostScreen = hostScreen;
        _account = account;
        _homeService = homeService;
        
        // Разрешить только если элемент выбран
        var canExe = this.WhenAnyValue(x => x.SelectedDepartments,
            (item) => item.Id > 0);
        // Загрузка всех отделов
        GetAllDepartment = ReactiveCommand.CreateFromTask(LoadedTreeAsync);
        // Получить список сотрудников при выбаре отдела
        GetPersonsByDepartment = ReactiveCommand.CreateFromTask(async _ => await GetPersonByTreeItemAsync(), canExe);
        GetPersonsByDepartment.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);

        NavigateToCard = ReactiveCommand.Create(() => { GoToPersonCard(account, SelectedPersons, SelectedDepartments); });
        
        NavigateToCard.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);
        // Фильтр по фио
        var filter = this.WhenValueChanged(x => x.FilterName).Select(Filter);
        _personsSourceList.Connect()
            .Filter(filter)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _personsList)
            .Subscribe();

        this.WhenActivated(disposables =>
        {
            GC.Collect();
            GetAllDepartment.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);
            GetAllDepartment.Execute()
                .Subscribe(x => Console.WriteLine("OnNext: {0}", x))
                .DisposeWith(disposables);
        });
    }
}

