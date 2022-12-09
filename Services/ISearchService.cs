using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;

namespace AvaloniaDesktop.Services;

public interface ISearchService
{
    public Task<ObservableCollection<Persons>> GetPersonsBySearch(Users user, string search);

    public Task<ObservableCollection<Departments>> GetDepartmentsBySearch(Users user, string search);

}