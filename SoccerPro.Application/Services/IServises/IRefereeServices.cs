using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RefereeDTOs;
using SoccerPro.Domain.Entities;

namespace SoccerPro.Application.Services.IServises;

public interface IRefereeServices
{
    public Task<Result<bool>> AddRefereeAsync(Referee referee, string usename, string IntialPassword);
    public Task<Result<List<RefereeViewDTO>>> GetAllRefereesAsync();
    public Task<Result<bool>> AssignRefereeToTournamentAsync(int refereeId, int tournamentId);
    public Task<Result<bool>> AssignRefereeToMatchAsync(int matchScheduleId, int tournamentRefereeId);

    public Task<Result<List<TournamentRefereeViewDTO>>> GetRefereesInTournamentAsync(int tournamentId);

}
