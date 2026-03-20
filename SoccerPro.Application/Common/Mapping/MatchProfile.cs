using AutoMapper;
using SoccerPro.Application.DTOs.MatchDTOs;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Application.Common.Mapping;

public class MatchProfile : Profile
{
    public MatchProfile()
    {
        CreateMap<MatchSchedule, MatchScheduleDTO>();

        CreateMap<MatchView, MatchDTO>();

    }
}