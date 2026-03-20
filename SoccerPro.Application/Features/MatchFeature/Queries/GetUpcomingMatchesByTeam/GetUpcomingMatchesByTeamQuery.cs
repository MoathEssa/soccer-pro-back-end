using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.MatchDTOs;

namespace SoccerPro.Application.Features.MatchFeature.Queries.GetUpcomingMatchesByTeam
{
    public record GetUpcomingMatchesByTeamQuery : IRequest<ApiResponse<IEnumerable<UpcomingMatchDTO>>>
    {
        public string? TeamName { get; init; }
        public string? TournamentName { get; init; }
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public GetUpcomingMatchesByTeamQuery(string? teamName, string? tournamentname = null, DateTime? fromDate = null, DateTime? toDate = null, int pageNumber = 1, int pageSize = 10)
        {
            TeamName = teamName;
            TournamentName = tournamentname;
            FromDate = fromDate;
            ToDate = toDate;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
