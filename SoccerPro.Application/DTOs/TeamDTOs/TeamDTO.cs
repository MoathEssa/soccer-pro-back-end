namespace SoccerPro.Application.DTOs.TeamDTOs;

public class TeamDTO
{
    public int TeamId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public int NumberOfPlayers { get; set; }
    public int ManagerId { get; set; }
    public string ManagerFirstName { get; set; } = string.Empty;
    public string ManagerLastName { get; set; } = string.Empty;
}