namespace SoccerPro.Application.DTOs.AuthDTOs;

public class RoleDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
}