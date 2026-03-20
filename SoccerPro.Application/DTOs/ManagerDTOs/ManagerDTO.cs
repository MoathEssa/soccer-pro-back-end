using SoccerPro.Application.DTOs.ContactInfoDTOs;

namespace SoccerPro.Application.DTOs.ManagerDTOs;

public class ManagerDTO
{
    public int ManagerId { get; set; }
    public string KFUPMId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? SecondName { get; set; }
    public string? ThirdName { get; set; }
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public int NationalityId { get; set; }
    public string? TeamName { get; set; }
    public List<ContactInfoDTO> PersonalContactInfos { get; set; } = [];
}