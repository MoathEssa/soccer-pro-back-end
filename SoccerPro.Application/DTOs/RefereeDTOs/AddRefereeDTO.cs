using SoccerPro.Application.DTOs.ContactInfoDTOs;

namespace SoccerPro.Application.DTOs.RefereeDTOs;

public class AddRefereeDTO
{


    public string KfupmId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string IntialPassword { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string SecondName { get; set; } = string.Empty;

    public string ThirdName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public int? NationalityId { get; set; }

    public required List<ContactInfoDTO> PersonalContactInfos { get; set; }

}

public class RefereeDTO
{
    public int RefereeId { get; set; }
    public string KfupmId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public string ThirdName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int NationalityId { get; set; }
}
