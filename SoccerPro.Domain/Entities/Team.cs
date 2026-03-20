namespace SoccerPro.Domain.Entities;

public class Team
{
    public int TeamId { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Website { get; set; } = null!;
    public int NumberOfPlayers { get; set; }
    public int ManagerId { get; set; }
    public List<TeamContactInfo> TeamContactInfo { get; set; } = new();
}