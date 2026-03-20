using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.ContactInfoDTOs;

namespace SoccerPro.Application.Features.SharedFeature.Queries.GetPersonalContactInfoByPersonId;

public record GetPersonalContactInfoByPersonIdQuery(int PersonId) : IRequest<Result<List<ContactInfoDTO>>>;
