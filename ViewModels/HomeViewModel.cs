using System.Collections.ObjectModel;
using System.Reactive;
using AvaloniaDesktop.Models;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AvaloniaDesktop.Services;
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
    
    private ObservableCollection<Persons> _personsList = new();
    public ObservableCollection<Persons> PersonsList
    {
        get => _personsList;
        set => this.RaiseAndSetIfChanged(ref _personsList, value);
    }
    
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
        // Получить список сотрудников при выбаре отдела
        GetPersonsByDepartment = ReactiveCommand.CreateFromTask(async _ => await GetPersonByTreeItemAsync(), canExe);
        GetPersonsByDepartment.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);
        
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadedTreeAsync();
        });
    }

    #region Команды

    public ReactiveCommand<Unit, Unit> GetPersonsByDepartment { get; }

    #endregion
    #region Логика

    private async void LoadedTreeAsync()
    {
        var departments = await _homeService!
            .GetTreeDepartments(_account);
        DepartmentsList = new ObservableCollection<Departments>(departments);
    }

    private async Task GetPersonByTreeItemAsync()
    {
        var personsByDepartment = await _homeService!
            .GetPersonsByDepartment(_account, SelectedDepartments);
        PersonsList = personsByDepartment;
    }

    #endregion
   

    
}

