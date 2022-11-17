using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Services;
using AvaloniaDesktop.Services.Api;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AvaloniaDesktop.ViewModels;

public sealed class PersonCardViewModel :  ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(PersonCardViewModel);
    public IScreen HostScreen { get; }
    
    private readonly ICardService? _cardService;

    #region Команды
    
    private ReactiveCommand<Unit, Unit> GetInforationByPerson { get; }
    private ReactiveCommand<Unit, Unit> GetPersonsByDepartments { get; }
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack { get; }
    public ReactiveCommand<Unit, Unit> GetAllPersons { get; }
    
    
    #endregion

    #region Свойства
    [Reactive] public string? FilterName { get; set; }

    private Persons _selectedPerson = new();

    public Persons SelectedPerson
    {
        get => _selectedPerson;
        set => this.RaiseAndSetIfChanged(ref _selectedPerson, value);
    }
    
    [Reactive] public Departments SelectedDepartments { get; set; }

    #endregion

    #region Свойства RadioButton
    
    [Reactive] public bool RadioIsMale { get; set; }
    
    [Reactive] public bool RadioIsFemale { get; set; }
    
    #endregion

    #region Массивы

    private readonly SourceList<Persons> _personsSourceList = new();
    
    private readonly ReadOnlyObservableCollection<Persons> _personsList;
    public ReadOnlyObservableCollection<Persons> PersonsList => _personsList;

    #endregion

    #region Логика
    
    // Информаци Личной карты сотрудника
    private async Task ApiGetInfo(Users users)
    {
        var information = await _cardService!.GetInformationByPerson(users, SelectedPerson);
        
        // SelectedPerson = information;
    }
    // Фильтр
    private static Func<Persons, bool> Filter(string? filterName)
    {
        if (string.IsNullOrEmpty(filterName)) return _ => true;
        return x => x.FullName.ToUpper().StartsWith(filterName.ToUpper());
    }

    // При переходе на персональную карту
    private async Task ApiGetListPersons(Users users, Departments departments , Persons persons)
    {
        var personsList = await _cardService!.GetShortNamePersonsByDepartmentId(users, departments.Id);
        _personsSourceList.Clear();
        _personsSourceList.AddRange(personsList);
        SelectedDepartments = departments;
        SelectedPerson = PersonsList.FirstOrDefault(x => x.Id == persons.Id)!;
    }
    
    // Отобразить всех сотрудников в sidebar
    private async Task ApiGetListAllPersonsAsync(Users users)
    {
        var personsList = await _cardService!.GetAllPersons(users);
        _personsSourceList.Clear();
        _personsSourceList.AddRange(personsList);

        SelectedPerson = PersonsList.FirstOrDefault()!;
    }
    

    #endregion
    
    

    public PersonCardViewModel(IScreen hostScreen , Users account , Persons selectedPersons, Departments selectedDepartment) :
        this(hostScreen,
            account,
            Locator.Current.GetService<ICardService>(),
            selectedPersons,
            selectedDepartment
        ) { }

    public PersonCardViewModel(IScreen hostScreen,
        Users account, 
        ICardService? getService,
        Persons selectedPersons,
        Departments selectedDepartment)
    {
        HostScreen = hostScreen;
        _cardService = getService;
        //SelectedPerson = selectedPersons;
        
        // Вернуться на главную страницу
        GoBack = HostScreen.Router.NavigateBack;
        // Загрузка сотрдуников на выбранный отдел
        GetPersonsByDepartments = ReactiveCommand.CreateFromTask(async _ => 
            await ApiGetListPersons(account , selectedDepartment , selectedPersons));
        // Загрузить всех сотрудников
        GetAllPersons = ReactiveCommand.CreateFromTask(async _ => await ApiGetListAllPersonsAsync(account));
        // Загрузить информацию о сотруднике при выборе
        GetInforationByPerson = ReactiveCommand.CreateFromTask(async _ => await ApiGetInfo(account));
        // Фильтр
        var filter = this.WhenValueChanged(x => x.FilterName).Select(Filter);
        _personsSourceList.Connect()
            .Filter(filter)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _personsList)
            .Subscribe();
            
        
       this.WhenActivated((CompositeDisposable disposables) =>
       {
           GC.Collect();
           
           GetPersonsByDepartments.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);
           GetPersonsByDepartments.Execute()
               .Subscribe(x => Console.WriteLine("OnNext: {0}", x))
               .DisposeWith(disposables);
       });
    }
}