using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.sharedDTOs;
using SoccerPro.Application.DTOs.SharedDTOs;
using SoccerPro.Application.DTOs.ContactInfoDTOs;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Services.IServises;
public interface ISharedServices
{
    Task<Result<List<CountryDTO>>> GetAllCountriesAsync();
    Task<Result<List<DepartmentDTO>>> GetAllDepartmentsAsync();
    Task<Result<List<PersonalContactInfo>>> GetPersonalContactInfoByPersonIdAsync(int personId);
}