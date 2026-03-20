namespace SoccerPro.Domain.Entities;

public class TournamentReferee
{
    public int TournamentRefereeId { get; set; }
    public int RefereeId { get; set; }
    public int TournamentId { get; set; } 

    // Navigation properties
    public Referee Referee { get; set; } = null!;
    public Tournament Tournament { get; set; } = null!;
}