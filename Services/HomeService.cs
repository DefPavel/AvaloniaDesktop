using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Services.Api;
using DynamicData;

namespace AvaloniaDesktop.Services;

public sealed class HomeService : IHomeSerivce
{
    private readonly SourceList<Departments> _itemsDepartments = new();
    public IObservable<IChangeSet<Departments>> Connect() => _itemsDepartments.Connect();

    public async Task<ReadOnlyObservableCollection<Departments>> GetTreeDepartments(Users user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
       return await QueryService.JsonDeserializeIObservable(user.Token, "/pers/tree/get", "GET");
    }
}