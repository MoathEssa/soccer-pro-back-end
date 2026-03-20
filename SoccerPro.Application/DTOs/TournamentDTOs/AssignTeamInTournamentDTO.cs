namespace SoccerPro.Application.DTOs.TournamentDTOs;

public class AssignTeamInTournamentDTO
{
    public int TournamentId { get; set; }
    public required int TeamId { get; set; }
}