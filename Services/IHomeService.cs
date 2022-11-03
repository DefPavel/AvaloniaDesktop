using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using DynamicData;

namespace AvaloniaDesktop.Services;

public interface IHomeService
{
    Task<ObservableCollection<Departments>> GetTreeDepartments(Users user);
    IObservable<IChangeSet<Departments>> Connect();
}