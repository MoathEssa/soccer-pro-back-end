using SoccerPro.Domain.Entities;
namespace SoccerPro.Application.Services.IServises;
public interface ISharedRepository
{
    Task<List<Country>> GetAllCountriesAsync();
    Task<List<Department>> GetAllDepartmentsAsync();
    Task<List<PersonalContactInfo>> GetPersonalContactInfoByPersonIdAsync(int personId);
}