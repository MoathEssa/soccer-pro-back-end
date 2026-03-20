namespace SoccerPro.Domain.Entities;

public class Tournament
{
    public int TournamentId { get; set; }
    public string Number { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}