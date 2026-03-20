using SoccerPro.Domain.Entities;

namespace SoccerPro.Domain.IRepository;

public interface ITournamentRepository
{
    public Task<bool> IsTeamInTournamentAsync(int teamId, int tournamentId);
    public Task<bool> TournamentExistsAsync(int tournamentId);
    public Task<int> AddTournamentAsync(Tournament tournament);
    public Task<Tournament?> GetTournamentByIdAsync(int tournamentId);
    public Task<bool> UpdateTournamentAsync(Tournament tournament);
    public Task<bool> DeleteTournamentAsync(int tournamentId);
    public Task<(List<Tournament> Tournaments, int TotalCount)> SearchTournamentsAsync(
            string? number, string? name, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
    public Task<bool> AssignTeamToTournamentAsync(int tournamentId, int teamIds);
}