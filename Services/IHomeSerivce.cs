using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using DynamicData;

namespace AvaloniaDesktop.Services;

public interface IHomeSerivce
{
    Task<ReadOnlyObservableCollection<Departments>> GetTreeDepartments(Users user);
    IObservable<IChangeSet<Departments>> Connect();
}