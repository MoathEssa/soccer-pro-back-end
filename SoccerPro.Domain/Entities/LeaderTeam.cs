namespace SoccerPro.Domain.Entities;

public class LeaderTeam
{
    public int LeaderTeamId { get; set; }
    public int LeaderId { get; set; }
    public int TeamId { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
}