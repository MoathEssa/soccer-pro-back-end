using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Application.DTOs.RefereeDTOs.Profile;

public class RefereeProfile : AutoMapper.Profile
{
    public RefereeProfile()
    {
        CreateMap<TournamentRefereeView, TournamentRefereeViewDTO>();

        CreateMap<TournamentReferee, RefereeInTournamentDTO>()
            .ForMember(dest => dest.KFUPMId, opt => opt.MapFrom(src => src.Referee.Person.KFUPMId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Referee.Person.FirstName))
            .ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.Referee.Person.SecondName))
            .ForMember(dest => dest.ThirdName, opt => opt.MapFrom(src => src.Referee.Person.ThirdName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Referee.Person.LastName))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Referee.Person.DateOfBirth))
            .ForMember(dest => dest.NationalityId, opt => opt.MapFrom(src => src.Referee.Person.NationalityId))
            .ForMember(dest => dest.TournamentName, opt => opt.MapFrom(src => src.Tournament.Name));
    }
}
