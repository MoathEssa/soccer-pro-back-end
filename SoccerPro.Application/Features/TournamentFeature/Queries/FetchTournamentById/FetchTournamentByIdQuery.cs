using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TournamentDTOs;

namespace SoccerPro.Application.Features.TournamentFeature.Queries.FetchTournamentById;

public record FetchTournamentByIdQuery(int TournamentId) : IRequest<ApiResponse<TournamentDTO>>;