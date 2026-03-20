namespace SoccerPro.Domain.Entities;

public class TournamentTeam
{
    public int TournamentTeamId { get; set; } // Primary Key
    public int TournamentId { get; set; } // Foreign Key to Tournament
    public int TeamId { get; set; } // Foreign Key to Team

    // Navigation properties
    public Tournament Tournament { get; set; } = null!;
    public Team Team { get; set; } = null!;
}