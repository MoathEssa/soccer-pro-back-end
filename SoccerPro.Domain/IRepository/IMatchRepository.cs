using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Domain.IRepository;

public interface IMatchRepository
{
    public Task<int> InsertFullMatchRecordAsync(
    MatchRecord matchRecord);
    public Task<bool> ValidateTournamentRefereesAsync(
int matchScheduleId,
List<int> tournamentRefereeIds);
    public Task<MatchSchedule?> GetMatchScheduleByIdAsync(int id);
    Task<bool> ScheduleMatchAsync(MatchSchedule match);
    public Task<(List<MatchView> Matches, int TotalCount)> SearchMatchesAsync(
int? tournamentId = null,
int? tournamentPhase = null,
string? teamAName = null,
string? teamBName = null,
string? fieldName = null,
DateTime? matchDate = null,
int pageNumber = 1,
int pageSize = 10);
    public Task<bool> MatchScheduleExistsAsync(int matchId);
    public Task<int?> InsertMatchRecordAsync(MatchRecord record);
    public Task InsertShotsOnGoalAsync(List<ShotOnGoal> shots);

    public Task<bool> ValidatePlayersInTeamInTournamentAsync(int tournamentId, int tournamentTeamId, List<int> playerIds);


    public Task<List<Match>> GetUpcomingMatchesByTeamAsync(
       string? teamName,
       string? tournamentName = null,
       DateTime? fromDate = null,
       DateTime? toDate = null,
       int pageNumber = 1,
       int pageSize = 10);
}