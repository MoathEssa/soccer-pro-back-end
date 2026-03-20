namespace SoccerPro.Application.DTOs.AuthDTOs;

public class RegsterAccountRequestDTO
{
    public required string KFUPMId { get; set; }
    public required string FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? ThirdName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }

}