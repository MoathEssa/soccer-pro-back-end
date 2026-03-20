using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;

namespace SoccerPro.Application.Features.TeamsFeature.Queries.FetchTeams;

public record FetchTeamsQuery(
    string? Name = null,
    string? Address = null,
    string? Website = null,
    int? NumberOfPlayers = null,
    int? ManagerId = null,
    string? ManagerFirstName = null,
    string? ManagerLastName = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ApiResponse<(List<TeamDTO> Teams, int TotalCount)>>;
