using SoccerPro.Application.DTOs.AuthDTOs;
using SoccerPro.Application.DTOs.sharedDTOs;
using SoccerPro.Domain.Entities;
namespace SoccerPro.Application.DTOs.SharedDTOs.Profile;

public class DepartmentsProfile : AutoMapper.Profile
{
    public DepartmentsProfile()
    {
        CreateMap<Country, CountryDTO>();
        CreateMap<Role, RoleDTO>();

        CreateMap<Department, DepartmentDTO>();
    }
}