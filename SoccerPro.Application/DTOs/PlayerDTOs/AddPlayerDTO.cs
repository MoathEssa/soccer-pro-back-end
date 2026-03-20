using SoccerPro.Application.DTOs.ContactInfoDTOs;
using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.DTOs.PlayerDTOs;
public class AddPlayerDTO
{
    public required string KFUPMId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string IntialPassword { get; set; } = string.Empty;
    public required string FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? ThirdName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public int? NationalityId { get; set; }
    public required PlayerType PlayerType { get; set; }
    public required int DepartmentId { get; set; }
    public required PlayerStatus PlayerStatus { get; set; }
    public required List<ContactInfoDTO> PersonalContactInfos { get; set; }

}
