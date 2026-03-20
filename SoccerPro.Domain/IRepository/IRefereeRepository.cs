using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Domain.IRepository;

public interface IRefereeRepository
{
    public Task<bool> IsRefereeInSameMatchAsync(int matchScheduleId, int tournamentRefereeId);

    public Task<List<TournamentRefereeView>> GetRefereesInTournamentAsync(int tournamentId);
    public Task<int?> AddRefereeAsync(
       Referee Referee);
    public Task<bool> AssignTournamentRefereeToMatchAsync(int matchScheduleId, int tournamentRefereeId);
    public Task<List<RefereeView>> GetAllRefereesAsync();
    public Task<bool> AssignRefereeToTournamentAsync(int tournamentId, int refereeId);
    public Task<bool> IsRefereeExistsAsync(int refereeId);
    public Task<bool> IsRefereeInTournamentAsync(int refereeId, int tournamentId);
}
