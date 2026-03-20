using AutoMapper;
using SoccerPro.Application.DTOs.RefereeDTOs;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Application.Common.Mapping;

public class RefereeProfile : Profile
{
    public RefereeProfile()
    {

        CreateMap<RefereeView, RefereeViewDTO>();



        CreateMap<AddRefereeDTO, Referee>()
        .ForMember(dest => dest.Person, opt => opt.MapFrom(src => new Person
        {
            KFUPMId = src.KfupmId,
            FirstName = src.FirstName,
            SecondName = src.SecondName,
            ThirdName = src.ThirdName,
            LastName = src.LastName,
            DateOfBirth = src.DateOfBirth,
            NationalityId = src.NationalityId ?? 0,
            PersonalContactInfos = src.PersonalContactInfos
            .Select(x => new PersonalContactInfo
            {
                Value = x.Value,
                ContactType = (ContactType)x.ContactType
            }).ToList()
        }));

        CreateMap<Referee, RefereeDTO>()
            .ForMember(dest => dest.KfupmId, opt => opt.MapFrom(src => src.Person.KFUPMId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
            .ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.Person.SecondName))
            .ForMember(dest => dest.ThirdName, opt => opt.MapFrom(src => src.Person.ThirdName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Person.DateOfBirth))
            .ForMember(dest => dest.NationalityId, opt => opt.MapFrom(src => src.Person.NationalityId))
            ;
    }
}
