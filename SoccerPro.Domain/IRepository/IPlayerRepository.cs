using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;
namespace Namespace.SoccerPro.Domain.IRepository;

public interface IPlayerRepository
{
    public Task<bool> IsUserAlreadyPlayerAsync(int userId);
    public Task<int?> AddPlayerAsync(Player player);
    public Task<(List<PlayerView> Players, int TotalCount)> GetAllPlayersAsync(int? playerId,
    string? kfupmId,
    int pageNumber,
    int pageSize);
    public Task<Player?> GetPlayerByIdAsync(int playerId);
    public Task<bool> AssignPlayerToTeamAsync(PlayerTeam playerTeams);

    public Task<bool> IsPlayerAlreadyAssignedAsync(int playerId, int tournamentId);

    public Task<List<TopScorerPlayerView>> GetTopScorersAsync(int pageNumber, int pageSize);

    public Task<List<PlayerViolationView>> GetPlayerViolationsAsync(int pageNumber, int pageSize, int? cardType);
}