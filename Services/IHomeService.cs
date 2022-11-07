using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Models.Types;
using DynamicData;

namespace AvaloniaDesktop.Services;

public interface IHomeService
{
    Task<ObservableCollection<Departments>> GetTreeDepartments(Users user);
    Task<ObservableCollection<Persons>> GetPersonsByDepartment(Users user, Departments departments);
    Task<IEnumerable<TypePosition>> GetAllPosition(Users user);
    Task<ObservableCollection<Position>> GetPositionsByDepartment(Users user, Departments departments);
}