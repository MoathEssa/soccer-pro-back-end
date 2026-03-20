using AutoMapper;
using SoccerPro.Application.DTOs.MatchDTOs;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Common.Mapping;

public class MatchResultProfile : Profile
{
    public MatchResultProfile()
    {
        CreateMap<TeamMatchRecordDTO, MatchRecord>();
        CreateMap<ShotOnGoalDTO, ShotOnGoal>();
    }
}