using AutoMapper;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.sharedDTOs;
using SoccerPro.Application.DTOs.SharedDTOs;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Application.Common.Errors;
using System.Net;

namespace SoccerPro.Application.Services;
public class SharedServices : ISharedServices
{
    private readonly ISharedRepository _sharedRepository;
    private readonly IMapper _mapper;

    public SharedServices(ISharedRepository sharedRepository, IMapper mapper)
    {
        _sharedRepository = sharedRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<CountryDTO>>> GetAllCountriesAsync()
    {
        var countries = await _sharedRepository.GetAllCountriesAsync();

        var countriesDTO = _mapper.Map<List<CountryDTO>>(countries);

        return Result<List<CountryDTO>>.Success(countriesDTO);

    }

    public async Task<Result<List<DepartmentDTO>>> GetAllDepartmentsAsync()
    {
        var departments = await _sharedRepository.GetAllDepartmentsAsync();
        var departmentDTOs = _mapper.Map<List<DepartmentDTO>>(departments);
        return Result<List<DepartmentDTO>>.Success(departmentDTOs);
    }

    public async Task<Result<List<PersonalContactInfo>>> GetPersonalContactInfoByPersonIdAsync(int personId)
    {
        try
        {
            var contactInfos = await _sharedRepository.GetPersonalContactInfoByPersonIdAsync(personId);
            if (contactInfos == null || !contactInfos.Any())
                return Result<List<PersonalContactInfo>>.Failure(
                    new Error("NotFound.PersonalContactInfo", "No contact information found for the specified person."), 
                    HttpStatusCode.NotFound);

            return Result<List<PersonalContactInfo>>.Success(contactInfos);
        }
        catch (Exception ex)
        {
            return Result<List<PersonalContactInfo>>.Failure(
                new Error("Internal.PersonalContactInfo", $"Failed to retrieve contact information: {ex.Message}"), 
                HttpStatusCode.InternalServerError);
        }
    }
}