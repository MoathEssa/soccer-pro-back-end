namespace SoccerPro.Domain.Entities;

public class UsersRoles
{
    public int UserRoleId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public DateTime AssignDate { get; set; }
    public required string AssignedByUser { get; set; }
}