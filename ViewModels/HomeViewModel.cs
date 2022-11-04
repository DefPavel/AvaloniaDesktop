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
    
    private int _countAll;
    public int CountAll
    {
        get => _countAll;
        set => this.RaiseAndSetIfChanged(ref _countAll, value);
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
        
        /*_personsList.WhenAnyValue(x => x.Count)
            .Subscribe((i) => _countAll = i);
        */

        this.WhenActivated((CompositeDisposable disposables) =>
        {
        });
    }
    #region Команды
    private ReactiveCommand<Unit, Unit> GetAllDepartment { get; }
    public ReactiveCommand<Unit, Unit> GetPersonsByDepartment { get; }

    #endregion
    
    #region Логика
    // Фильтр по Ф.И.О.
    private static Func<Persons, bool> Filter(string? filterName)
    {
        if (string.IsNullOrEmpty(filterName)) return x => true;
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
        var personsByDepartment = await _homeService!
            .GetPersonsByDepartment(_account, SelectedDepartments);
        
        _personsSourceList.Clear();
        _personsSourceList.AddRange(personsByDepartment);

    }
    #endregion
   

    
}

