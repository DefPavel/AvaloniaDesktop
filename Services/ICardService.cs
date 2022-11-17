using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaDesktop.Models;

namespace AvaloniaDesktop.Services;

public interface ICardService
{
    Task<ObservableCollection<Persons>> GetShortNamePersonsByDepartmentId(Users user, int idDepartment);

    Task<ObservableCollection<Persons>> GetAllPersons(Users user);
    
    Task<Persons> GetInformationByPerson(Users user, Persons persons);


}