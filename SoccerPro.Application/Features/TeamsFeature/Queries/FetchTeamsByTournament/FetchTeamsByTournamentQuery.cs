using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;

namespace SoccerPro.Application.Features.TeamsFeature.Queries.FetchTeamsByTournament;

public record FetchTeamsByTournamentQuery(
    int TournamentId,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ApiResponse<List<TeamTournamentViewDTO>>>;
