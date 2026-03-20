namespace SoccerPro.Domain.Entities;

public class CoachTeam
{
    public int CoachTeamId { get; set; }
    public int CoachId { get; set; }
    public int TeamId { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
}
