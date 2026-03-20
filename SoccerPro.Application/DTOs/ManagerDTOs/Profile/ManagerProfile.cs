using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Application.DTOs.ManagerDTOs.Profile;

public class ManagerProfile : AutoMapper.Profile
{
    public ManagerProfile()
    {

        CreateMap<ManagerSearchView, ManagerSearchViewDTO>();

        CreateMap<AddManagerDTO, Manager>()
            .ForMember(dest => dest.Person, opt => opt.MapFrom(src => new Person
            {
                KFUPMId = src.KFUPMId,
                FirstName = src.FirstName,
                SecondName = src.SecondName,
                ThirdName = src.ThirdName,
                LastName = src.LastName,
                DateOfBirth = src.DateOfBirth,
                NationalityId = src.NationalityId ?? null,
                PersonalContactInfos = src.PersonalContactInfos.Select(c => new PersonalContactInfo
                {
                    ContactType = (ContactType)c.ContactType,
                    Value = c.Value
                }).ToList()
            }));

        CreateMap<ManagerView, ManagerDTO>();

        CreateMap<ManagerView, ManagerViewDTO>()
            .ForMember(dest => dest.ContactInfosDTO, opt => opt.MapFrom(src => src.PersonalContactInfo));

        CreateMap<Manager, ManagerDTO>()
            .ForMember(dest => dest.KFUPMId, opt => opt.MapFrom(src => src.Person.KFUPMId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
            .ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.Person.SecondName))
            .ForMember(dest => dest.ThirdName, opt => opt.MapFrom(src => src.Person.ThirdName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Person.DateOfBirth))
            .ForMember(dest => dest.NationalityId, opt => opt.MapFrom(src => src.Person.NationalityId))
            .ForMember(dest => dest.PersonalContactInfos, opt => opt.MapFrom(src => src.Person.PersonalContactInfos));
    }
}