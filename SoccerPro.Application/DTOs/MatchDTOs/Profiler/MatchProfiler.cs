using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.DTOs.MatchDTOs.Profiler
{
    public class MatchProfiler : AutoMapper.Profile
    {
        public MatchProfiler()
        {
            CreateMap<AddMatchRecordDTO, MatchRecord>()
                .ForMember(dest => dest.MatchScheduleId, opt => opt.MapFrom(src => src.MatchScheduleId))
                .ForMember(dest => dest.TournamentTeamId, opt => opt.MapFrom(src => src.TeamRecord.TournamentTeamId))
                .ForMember(dest => dest.GoalsFor, opt => opt.MapFrom(src => src.TeamRecord.GoalsFor))
                .ForMember(dest => dest.GoalAgainst, opt => opt.MapFrom(src => src.TeamRecord.GoalAgainst))
                .ForMember(dest => dest.AcquisitionRate, opt => opt.MapFrom(src => src.TeamRecord.AcquisitionRate))
                .ForMember(dest => dest.ShotsOnGoal, opt => opt.MapFrom(src => src.TeamRecord.ShotsOnGoal))
                .ForMember(dest => dest.CardViolations, opt => opt.MapFrom(src => src.TeamRecord.CardsViolations))
                .ForMember(dest => dest.MatchSubstitutions, opt => opt.MapFrom(src => src.TeamRecord.matchSubstitutionDTOs));



            CreateMap<ShotOnGoalDTO, ShotOnGoal>()
                .ForMember(dest => dest.ShotType, opt => opt.MapFrom(src => (ShotType)src.ShotType))
                ;

            CreateMap<CardViolationDTO, CardViolation>()
                  .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => (CardType)src.CardType));


            CreateMap<MatchSubstitutionDTO, MatchSubstitution>()
                    .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => (SubstitutionReason)src.Reason));


        }
    }
}
