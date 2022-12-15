using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Services;
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
    
    public ReactiveCommand<Unit, Unit> GetInforationByPerson { get; }
    private ReactiveCommand<Unit, Unit> GetPersonsByDepartments { get; }
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack { get; }
    public ReactiveCommand<Unit, Unit> GetAllPersons { get; }
    
    
    #endregion

    #region Свойства
    
    [Reactive] public string? FullAge { get; set; }
    [Reactive] public string? FilterName { get; set; }
    [Reactive] public Persons? SelectedPerson { get; set; }
    [Reactive] public Position? SelectedPosition { get; set; }
    [Reactive] public Persons? InforamationPerson { get; set; }
    
    // [Reactive] public Departments? SelectedDepartments { get; set; }
    [Reactive] public string? TitleDepartment { get; set; }

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
        try
        {
            var information = await _cardService!.GetInformationByPerson(users, SelectedPerson!);

            InforamationPerson = information;
            TitleDepartment = information.ArrayPosition[0].DepartmentName;
            // Полный возраст сотрудника 
            FullAge = GetFullAge(information);
        }
        // Ошибка токена
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Предупреждение",
                    "Вы не проявляли активности в программе более 30 минут!");
            await messageBoxStandardWindow.Show();

            HostScreen.Router.NavigateAndReset.Execute(new LoginViewModel(HostScreen));
        }
        // Ошибка с сервера
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());
                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Ошибочка", await reader.ReadToEndAsync());
                    await messageBoxStandardWindow.Show();
                }
            }
        }
        // Что-то опасное
        catch (Exception ex)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибочка", ex.Message);
            await messageBoxStandardWindow.Show();
        }
       
    }
    // Фильтр
    private static Func<Persons, bool> Filter(string? filterName)
    {
        if (string.IsNullOrEmpty(filterName)) return _ => true;
        return x => x.FullName.ToUpper().StartsWith(filterName.ToUpper());
    }

    // Интерполяция возраста
    private static string GetFullAge(Persons informationTask) => 
        $"Лет:{informationTask.FullAge?.Years}; Месяцев:{informationTask.FullAge?.Months}; Дней:{informationTask.FullAge?.Days};";
    

    // При переходе на персональную карту
    private async Task ApiGetListPersons(Users users, Departments departments , Persons persons)
    {
        try
        {
            var personsListTask = _cardService!.GetShortNamePersonsByDepartmentId(users, departments.Id);
            var informationTask = _cardService!.GetInformationByPerson(users, persons!);
            await Task.WhenAll(personsListTask, informationTask);
        
            _personsSourceList.Clear();
            _personsSourceList.AddRange(personsListTask.Result.DistinctBy(x => x.Id));
            TitleDepartment = departments.Name;
            // Выбрать нужный элемент
            SelectedPerson = PersonsList.FirstOrDefault(x => x.Id == persons.Id)!;
            // Информация о персоне
            InforamationPerson = informationTask.Result;
            // Полный возраст сотрудника 
            FullAge = GetFullAge(informationTask.Result);
        }
        // Ошибка токена
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Предупреждение",
                    "Вы не проявляли активности в программе более 30 минут!");
            await messageBoxStandardWindow.Show();

            HostScreen.Router.NavigateAndReset.Execute(new LoginViewModel(HostScreen));
        }
        // Ошибка с сервера
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());
                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Ошибочка", await reader.ReadToEndAsync());
                    await messageBoxStandardWindow.Show();
                }
            }
        }
        // Что-то опасное
        catch (Exception ex)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибочка", ex.Message);
            await messageBoxStandardWindow.Show();
        }
    }
    
    // Отобразить всех сотрудников в sidebar
    private async Task ApiGetListAllPersonsAsync(Users users)
    {
        try
        {
            var personsList = await _cardService!.GetAllPersons(users);
            _personsSourceList.Clear();
            _personsSourceList.AddRange(personsList.DistinctBy(x => x.Id));

            SelectedPerson = PersonsList.FirstOrDefault()!;
        }
        // Ошибка токена
        catch (WebException ex) when ((int)(ex.Response as HttpWebResponse)!.StatusCode == 419)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Предупреждение",
                    "Вы не проявляли активности в программе более 30 минут!");
            await messageBoxStandardWindow.Show();

            HostScreen.Router.NavigateAndReset.Execute(new LoginViewModel(HostScreen));
        }
        // Ошибка с сервера
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    using StreamReader reader = new(response.GetResponseStream());
                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Ошибочка", await reader.ReadToEndAsync());
                    await messageBoxStandardWindow.Show();
                }
            }
        }
        // Что-то опасное
        catch (Exception ex)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибочка", ex.Message);
            await messageBoxStandardWindow.Show();
        }
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
            .Throttle(TimeSpan.FromMilliseconds(400))
            .Subscribe(async _ => await ApiGetInfo(account));
            
        
       this.WhenActivated(disposables =>
       {
           GC.Collect();
           
           GetPersonsByDepartments.IsExecuting.ToProperty(this, x => x.IsBusy, out isBusy);
           GetPersonsByDepartments.Execute()
               .Subscribe(x => Console.WriteLine("OnNext: {0}", x))
               .DisposeWith(disposables);
       });
    }
}