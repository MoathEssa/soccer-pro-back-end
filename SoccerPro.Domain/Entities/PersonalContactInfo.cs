namespace SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;


public class PersonalContactInfo
{
    public int PersonalContactInfoId { get; set; }
    public int PersonId { get; set; }
    public ContactType ContactType { get; set; } 
    public required string Value { get; set; } 
}