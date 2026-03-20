using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.MatchDTOs;
using SoccerPro.Domain.IRepository;

namespace SoccerPro.Application.Features.QueriesFeature.GetUpcomingMatchesByTeam
{
    public class GetUpcomingMatchesByTeamQueryHandler : IRequestHandler<GetUpcomingMatchesByTeamQuery, ApiResponse<IEnumerable<UpcomingMatchDTO>>>
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;

        public GetUpcomingMatchesByTeamQueryHandler(IMatchRepository matchRepository, IMapper mapper)
        {
            _matchRepository = matchRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<UpcomingMatchDTO>>> Handle(GetUpcomingMatchesByTeamQuery request, CancellationToken cancellationToken)
        {
            var matches = await _matchRepository.GetUpcomingMatchesByTeamAsync(
                request.TeamName,
                request.TournamentName,
                request.FromDate,
                request.ToDate,
                request.PageNumber,
                request.PageSize);

            var mappedMatches = _mapper.Map<IEnumerable<UpcomingMatchDTO>>(matches);
            return ApiResponseHandler.Success(mappedMatches);
        }
    }
}
