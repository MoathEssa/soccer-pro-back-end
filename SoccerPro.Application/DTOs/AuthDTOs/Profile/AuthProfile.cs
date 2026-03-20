using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.DTOs.AuthDTOs.Profile
{
    public class AuthProfile : AutoMapper.Profile
    {
        public AuthProfile()
        {
            CreateMap<Role, RoleDTO>();


            //      CreateMap<RegisterUserDTO, User>()
            //.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            //.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            //.ForPath(dest => dest.Person.KFUPMId, opt => opt.MapFrom(src => src.KFUPMId))
            //.ForPath(dest => dest.Person.FirstName, opt => opt.MapFrom(src => src.FirstName))
            //.ForPath(dest => dest.Person.SecondName, opt => opt.MapFrom(src => src.SecondName))
            //.ForPath(dest => dest.Person.ThirdName, opt => opt.MapFrom(src => src.ThirdName))
            //.ForPath(dest => dest.Person.LastName, opt => opt.MapFrom(src => src.LastName))
            //.ForPath(dest => dest.Person.PersonalContactInfos, opt => opt.Ignore())
            //.ForPath(dest => dest.Person.DateOfBirth, opt => opt.Ignore())
            //.ForPath(dest => dest.Person.NationalityId, opt => opt.Ignore())
            //.ForPath(dest => dest.Person.PersonId, opt => opt.Ignore());
        }
    }
}
