using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Services.Api;

namespace AvaloniaDesktop.Services;

public class SearchService : ISearchService
{
    public async Task<ObservableCollection<Persons>> GetPersonsBySearch(Users user , string search)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        return await QueryService.JsonDeserializeObservable<Persons>(user.Token, $"/pers/person/find/?text={search.Trim()}", "GET");
    }
    
    public async Task<ObservableCollection<Departments>> GetDepartmentsBySearch(Users user , string search)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        return await QueryService.JsonDeserializeObservable<Departments>(user.Token, $"/pers/tree/find/?text={search.Trim()}", "GET");
    }
}