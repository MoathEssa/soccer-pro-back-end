namespace SoccerPro.Application.Services.IServises;

using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

public interface IPlayerServices
{
    public Task<Result<bool>> AddPlayerAsync(Player player, string username, string IntialPassword);


    Task<Result<List<TopScorerPlayerView>>> GetTopScorersAsync(int pageNumber, int pageSize);
    Task<Result<List<PlayerViolationView>>> GetPlayerViolationsAsync(int pageNumber, int pageSize, int? cardType);


    public Task<Result<(List<PlayerDTO> playerDTOs, int totalCount)>> GetAllPlayersAsync(int? playerId,
    string? kfupmId,
    int pageNumber = 1,
    int pageSize = 10);


    public Task<Result<PlayerDTO>> GetPlayerByIdAsync(int playerId);

    public Task<Result<bool>> AssignPlayerToTeamAsync(PlayerTeam playerTeam, int tournamentId);

}