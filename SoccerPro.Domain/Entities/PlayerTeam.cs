using SoccerPro.Domain.Enums;

namespace SoccerPro.Domain.Entities;

public class PlayerTeam
{
    public int PlayerTeamId { get; set; }
    public int PlayerId { get; set; }
    public int TeamId { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public PlayerPosition PlayerPosition { get; set; }
    public PlayerRole PlayerRole { get; set; }
}