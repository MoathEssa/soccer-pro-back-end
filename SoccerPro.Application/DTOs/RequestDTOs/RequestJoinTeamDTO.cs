namespace SoccerPro.Application.DTOs.RequestDTOs;

public class RequestJoinTeamDTO
{
    public int UserId { get; set; }
    public int TeamId { get; set; }
    public int PlayerPosition { get; set; }
    public int PlayerRole { get; set; }
    public int PlayerType { get; set; }
    public int DepartmentId { get; set; }
    public string? Notes { get; set; }
}