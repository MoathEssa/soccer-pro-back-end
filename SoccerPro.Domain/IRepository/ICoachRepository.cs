using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Domain.IRepository;

public interface ICoachRepository
{
    Task<int?> AddCoachAsync(Coache coach);

    public Task<bool> AssignCoachToTeamAsync(CoachTeam coachTeam);
    Task<(List<CoachView> coaches, int totalCount)> GetCoachesAsync(
     string? kfupmId = null,
     string? firstName = null,
     string? secondName = null,
     string? thirdName = null,
     string? lastName = null,
     DateTime? dateOfBirth = null,
     int? nationalityId = null,
     string? teamName = null,
     bool? isActive = null,
     int pageNumber = 1,
     int pageSize = 10);

    public Task<bool> IsCoachAlreadyAssignedAsync(int coachId, int tournamentId);
}