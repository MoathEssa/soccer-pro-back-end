using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TournamentDTOs;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.AssignTeamsInTournament;

public record AssignTeamInTournamentCommand(AssignTeamInTournamentDTO AssignTeamsInTournamentDTO) : IRequest<ApiResponse<bool>>;