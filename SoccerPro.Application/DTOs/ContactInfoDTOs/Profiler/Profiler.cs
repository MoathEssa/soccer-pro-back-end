namespace SoccerPro.Application.DTOs.ContactInfoDTOs.Profiler
{
    public class Profiler : AutoMapper.Profile
    {
        public Profiler()
        {
            CreateMap<ContactInfoDTO, Domain.Entities.TeamContactInfo>()
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => (Domain.Entities.Enums.ContactType)src.ContactType));


            CreateMap<Domain.Entities.PersonalContactInfo, ContactInfoDTO>()
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => (int)src.ContactType));


            CreateMap<Domain.Entities.TeamContactInfo, ContactInfoDTO>()
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => (int)src.ContactType));


            CreateMap<ContactInfoDTO, Domain.Entities.TeamContactInfo>()
                  .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => (Domain.Entities.Enums.ContactType)src.ContactType));

        }

    }
}
