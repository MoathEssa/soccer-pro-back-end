using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TournamentDTOs;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Services.IServises;

public interface ITournamentServices
{
    public Task<Result<bool>> AddTournamentAsync(Tournament tournament);
    public Task<Result<TournamentDTO>> GetTournamentByIdAsync(int tournamentId);

    public Task<Result<(List<TournamentDTO> tournaments, int totalCount)>> GetAllTournamentsAsync(
    string? number = null,
    string? name = null,
    DateTime? startDate = null,
    DateTime? endDate = null,
    int pageNumber = 1,
    int pageSize = 10);

    public Task<Result<bool>> UpdateTournamentAsync(Tournament tournament);
    public Task<Result<bool>> DeleteTournamentAsync(int tournamentId);

    public Task<Result<bool>> AssignTeamToTournamentAsync(int tournamentId, int teamId);
}