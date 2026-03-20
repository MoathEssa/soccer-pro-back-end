using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TournamentDTOs;
using SoccerPro.Application.Services.IServises;

namespace SoccerPro.Application.Features.TournamentFeature.Queries.FetchTournamentById
{
    public class FetchTournamentByIdQueryHandler : IRequestHandler<FetchTournamentByIdQuery, ApiResponse<TournamentDTO>>
    {
        private readonly ITournamentServices _tournamentServices;

        public FetchTournamentByIdQueryHandler(ITournamentServices tournamentServices)
        {
            _tournamentServices = tournamentServices;
        }

        public async Task<ApiResponse<TournamentDTO>> Handle(FetchTournamentByIdQuery request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentServices.GetTournamentByIdAsync(request.TournamentId);

            return ApiResponseHandler.Build(
                data: tournament.Value,
                statusCode: tournament.StatusCode,
                succeeded: tournament.IsSuccess,
                errors: [tournament.Error.Message]
            );


        }
    }
}
