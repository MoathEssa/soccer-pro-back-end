using AutoMapper;
using SoccerPro.Application.DTOs.SharedDTOs;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Common.Mapping;

public class FieldProfile : Profile
{
    public FieldProfile()
    {
        CreateMap<Field, FieldDTO>();
    }
}