namespace SoccerPro.Application.DTOs.TeamDTOs;

public class UpdateTeamDTO
{
    public int TeamId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Website { get; set; }
    public required int NumberOfPlayers { get; set; }
}