namespace SoccerPro.Application.DTOs.PlayerDTOs;

public class AssignPlayerIntoTeamDTO
{

    public int TournamentId { get; set; }
    public int TeamId { get; set; }
    public int PlayerId { get; set; }
    public int Position { get; set; }
    public int Role { get; set; }
}

