using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Domain.Entities;

public class TeamCaptain
{
    public int MatchCaptainId { get; set; }
    public int TeamId { get; set; }
    public int PlayerId { get; set; } 
    public CaptainType CaptainType { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } 
}