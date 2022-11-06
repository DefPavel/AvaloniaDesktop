using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using AvaloniaDesktop.Models;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AvaloniaDesktop.Models.Types;
using AvaloniaDesktop.Services;
using DynamicData;
using DynamicData.Binding;
using Splat;

namespace AvaloniaDesktop.ViewModels;

public sealed class HomeViewModel : ViewModelBase, IRoutableViewModel
{
    #region Свойства

    private ObservableCollection<Departments> _departmentsList = new();
    public ObservableCollection<Departments> DepartmentsList
    {
        get => _departmentsList;
        set => this.RaiseAndSetIfChanged(ref _departmentsList, value);
    }
    
    private Departments _selectedDepartments = new();
    public Departments SelectedDepartments
    {
        get => _selectedDepartments;
        set => this.RaiseAndSetIfChanged(ref _selectedDepartments, value);
    }

    private string _filterName = string.Empty;
    public string FilterName
    {
        get => _filterName;
        set => this.RaiseAndSetIfChanged(ref _filterName, value);
    }

    #region Расчет количества сотрудников

    private int _countAllPerson;
    public int CountAllPerson
    {
        get => _countAllPerson;
        set => this.RaiseAndSetIfChanged(ref _countAllPerson, value);
    }
    
    private int _countIsPedPerson;
    public int CountIsPedPerson
    {
        get => _countIsPedPerson;
        set => this.RaiseAndSetIfChanged(ref _countIsPedPerson, value);
    }
    
    private int _countNotIsPedPerson;
    public int CountNotIsPedPerson
    {
        get => _countNotIsPedPerson;
        set => this.RaiseAndSetIfChanged(ref _countNotIsPedPerson, value);
    }
    
    private int _countIsPluralismInnerIsPed;
    public int CountIsPluralismInnerIsPed
    {
        get => _countIsPluralismInnerIsPed;
        set => this.RaiseAndSetIfChanged(ref _countIsPluralismInnerIsPed, value);
    }
    
    private int _countIsPluralismInnerNotIsPed;
    public int CountIsPluralismInnerNotIsPed
    {
        get => _countIsPluralismInnerNotIsPed;
        set => this.RaiseAndSetIfChanged(ref _countIsPluralismInnerNotIsPed, value);
    }
    
    private int _countIsPluralismOterIsPed;
    public int CountIsPluralismOterIsPed
    {
        get => _countIsPluralismOterIsPed;
        set => this.RaiseAndSetIfChanged(ref _countIsPluralismOterIsPed, value);
    }
    
    private int _countIsPluralismOterNotIsPed;
    public int CountIsPluralismOterNotIsPed
    {
        get => _countIsPluralismOterNotIsPed;
        set => this.RaiseAndSetIfChanged(ref _countIsPluralismOterNotIsPed, value);
    }
    

    #endregion
    
    private IEnumerable<string>? _typePosition;
    public IEnumerable<string>? TypePosition
    {
        get => _typePosition;
        set => this.RaiseAndSetIfChanged(ref _typePosition, value);
    }
    
    private string? _filterType;
    public string? FilterType
    {
        get => _filterType;
        set => this.RaiseAndSetIfChanged(ref _filterType, value);
    }
    
    
    
    private ObservableCollection<Position>? _positionsList;
    public ObservableCollection<Position>? PositionsList
    {
        get => _positionsList;
        set => this.RaiseAndSetIfChanged(ref _positionsList, value);
    }
    
    private Position _selectedPosition = new();
    public Position SelectedPosition
    {
        get => _selectedPosition;
        set => this.RaiseAndSetIfChanged(ref _selectedPosition, value);
    }
    
    private readonly SourceList<Persons> _personsSourceList = new();

    private readonly ReadOnlyObservableCollection<Persons> _personsList;
    public ReadOnlyObservableCollection<Persons> PersonsList => _personsList;

    private Persons _selectedPersons = new();
    public Persons SelectedPersons
    {
        get => _selectedPersons;
        set => this.RaiseAndSetIfChanged(ref _selectedPersons, value);
    }
    public string UrlPathSegment => nameof(HomeViewModel);
    private readonly IHomeService? _homeService;
    private readonly Users _account;
    public IScreen HostScreen { get; }
    #endregion
    
     #region Команды
    private ReactiveCommand<Unit, Unit> GetAllDepartment { get; }
    public ReactiveCommand<Unit, Unit> GetPersonsByDepartment { get; }

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
    }
    private async Task GetPersonByTreeItemAsync()
    {
        // получить список сотрудников выбранного отдела
        var personsByDepartment = await _homeService!
            .GetPersonsByDepartment(_account, SelectedDepartments);
        // получить все должности
        var allPositions = await _homeService!.GetAllPosition(_account);

        TypePosition = allPositions.Select(VARIABLE => VARIABLE.Name).ToArray();
        // получить штатные должности выбранного отдела
        var positionByDepartment = await _homeService!.GetPositionsByDepartment(_account, SelectedDepartments);
        PositionsList = positionByDepartment;
        
        _personsSourceList.Clear();
        _personsSourceList.AddRange(personsByDepartment);

        CountAllPerson = _personsList.DistinctBy(x => x.Id).Count();
        // && x.IsMain == true
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
        /*CountFreeBudget = Positions!.Sum(x => x.Free_B);
        // Считаем свободных внебюджетных ставки
        CountFreeNotBudget = Positions!.Sum(x => x.Free_NB);
        // итоги бюджетных ставок
        CountBudget = _personsList.Sum(x => x.StavkaBudget);
        // итоги внебюджетных ставок
        CountNotBudget = _personsList.Sum(x => x.StavkaNoBudget);
        */

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
        GetAllDepartment.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);
        GetAllDepartment.Execute()
            .Subscribe(x => Console.WriteLine("OnNext: {0}", x))
            .Dispose();
        // Получить список сотрудников при выбаре отдела
        GetPersonsByDepartment = ReactiveCommand.CreateFromTask(async _ => await GetPersonByTreeItemAsync(), canExe);
        GetPersonsByDepartment.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);
       
        // Фильтр по фио
        var filter = this.WhenValueChanged(x => x.FilterName).Select(Filter);
        _personsSourceList.Connect().Filter(filter).ObserveOn(RxApp.MainThreadScheduler).Bind(out _personsList).Subscribe();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
        });
    }
}

