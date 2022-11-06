using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Models.Types;
using AvaloniaDesktop.Services.Api;
using DynamicData;

namespace AvaloniaDesktop.Services;

public sealed class HomeService : IHomeService
{
    private readonly SourceList<Departments> _itemsDepartments = new();
    public IObservable<IChangeSet<Departments>> Connect() => _itemsDepartments.Connect();

    public async Task<ObservableCollection<Departments>> GetTreeDepartments(Users user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
       return await QueryService.JsonDeserializeObservable<Departments>(user.Token, "/pers/tree/get", "GET");
    }
    
    public async Task<ObservableCollection<TypePosition>> GetAllPosition(Users user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        return await QueryService.JsonDeserializeObservable<TypePosition>(user.Token, "/pers/position/type/position/", "GET");
    }
    
    public async Task<ObservableCollection<Position>> GetPositionsByDepartment(Users user , Departments departments)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        return await QueryService.JsonDeserializeObservable<Position>(user.Token, "/pers/position/get/" + departments.Id, "GET");
    }

    public async Task<ObservableCollection<Persons>> GetPersonsByDepartment(Users user , Departments departments)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        return await QueryService.JsonDeserializeObservable<Persons>(user.Token, "/pers/person/get/department/" + departments.Id, "GET");
    }
}