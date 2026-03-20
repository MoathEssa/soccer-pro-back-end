using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Domain.IRepository;

public interface ITeamRepository
{

    public Task<(List<TeamTournamentView> teams, int totalCount)> GetTeamsByTournamentIdAsync(
          int tournamentId,
          int pageNumber = 1,
          int pageSize = 10);



    public Task<bool> IsTeamInTournamentAsync(int TournamentTeamId);


    public Task<bool> TeamExistsAsync(int teamId);
    Task<bool> AddTeamAsync(Team team);
    Task<bool> DeleteTeamAsync(int teamId);
    public Task<(List<TeamView> teams, int totalCount)> SearchTeamsAsync(
        string? name = null,
        string? address = null,
        string? website = null,
        int? numberOfPlayers = null,
        int? managerId = null,
        string? managerFirstName = null,
        string? managerLastName = null,
        int pageNumber = 1,
        int pageSize = 10);
    Task<TeamView?> GetTeamByIdAsync(int teamId);
    Task<bool> UpdateTeamAsync(Team team);

    public Task<bool> IsTeamInTournamentAsync(int teamId, int tournamentId);



}