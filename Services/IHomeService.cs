using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using DynamicData;

namespace AvaloniaDesktop.Services;

public interface IHomeService
{
    Task<ObservableCollection<Departments>> GetTreeDepartments(Users user);
    Task<ObservableCollection<Persons>> GetPersonsByDepartment(Users user, Departments departments);
    IObservable<IChangeSet<Departments>> Connect();
}