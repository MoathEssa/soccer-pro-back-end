using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Services.IServises;

public interface ITeamServices
{
    Task<Result<bool>> AddTeamAsync(Team team);
    Task<Result<TeamViewDTO>> GetTeamByIdAsync(int teamId);
    Task<Result<(List<TeamDTO> teams, int totalCount)>> SearchTeamsAsync(
     string? name = null,
     string? address = null,
     string? website = null,
     int? numberOfPlayers = null,
     int? managerId = null,
     string? managerFirstName = null,
     string? managerLastName = null,
     int pageNumber = 1,
     int pageSize = 10);

    Task<Result<bool>> UpdateTeamAsync(Team team);
    Task<Result<bool>> DeleteTeamAsync(int teamId);

    Task<Result<(List<TeamTournamentViewDTO> teams, int totalCount)>> GetTeamsByTournamentAsync(
        int tournamentId,
        int pageNumber = 1,
        int pageSize = 10);
}