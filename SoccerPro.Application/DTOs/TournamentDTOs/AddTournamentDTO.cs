namespace SoccerPro.Application.DTOs.TournamentDTOs;

public class AddTournamentDTO
{
    public required string Name { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}