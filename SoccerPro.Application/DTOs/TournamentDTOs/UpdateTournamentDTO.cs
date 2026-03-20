namespace SoccerPro.Application.DTOs.TournamentDTOs;

public class UpdateTournamentDTO
{
    public int TournamentId { get; set; }
    public required string Name { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}