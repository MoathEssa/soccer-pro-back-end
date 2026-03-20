namespace SoccerPro.Application.DTOs.TeamDTOs;

public class AssignCoachIntoTeamDTO
{
    public int TournamentId { get; set; }
    public int TeamId { get; set; }
    public int CoachId { get; set; }
}