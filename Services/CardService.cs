using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;
using AvaloniaDesktop.Models.Types;
using AvaloniaDesktop.Services.Api;

namespace AvaloniaDesktop.Services;

public class CardService : ICardService
{

    public async Task<ObservableCollection<Persons>> GetShortNamePersonsByDepartmentId(Users user , int idDepartment )
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        return await QueryService.JsonDeserializeObservable<Persons>(token: user!.Token, "/pers/person/get/short/" + idDepartment, "GET");
    }

    public async Task<ObservableCollection<Persons>> GetAllPersons(Users user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        return await QueryService.JsonDeserializeObservable<Persons>(token: user!.Token, "/pers/person/get/all", "GET");
    }
}