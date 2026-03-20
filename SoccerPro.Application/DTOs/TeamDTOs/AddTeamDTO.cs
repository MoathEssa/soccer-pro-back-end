namespace SoccerPro.Application.DTOs.TeamDTOs;

public class AddTeamDTO
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Website { get; set; }
    public required int NumberOfPlayers { get; set; }
    public required int ManagerId { get; set; }
    public required List<ContactInfoDTOs.ContactInfoDTO> ContactInfo { get; set; }
}