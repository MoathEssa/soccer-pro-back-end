using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RefereeDTOs;

namespace SoccerPro.Application.Features.RefereeFeature.Queries.GetAllRefereesInTournament;

public record GetAllRefereesInTournamentQuery(int TournamentId) : IRequest<ApiResponse<List<TournamentRefereeViewDTO>>>;
