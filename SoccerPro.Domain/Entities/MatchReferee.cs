namespace SoccerPro.Domain.Entities;

public class MatcheReferee
{
    public int MatchRefereeId { get; set; } // Primary Key
    public int MatchScheduleId { get; set; } // Foreign Key to MatchSchedule
    public int TournamentRefereeId  { get; set; } // Foreign Key to Referees

    // Navigation properties
    public MatchSchedule MatchSchedule { get; set; } = null!;
    public Referee Referee { get; set; } = null!;
}