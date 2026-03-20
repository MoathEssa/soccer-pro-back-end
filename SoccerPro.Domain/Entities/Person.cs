namespace SoccerPro.Domain.Entities;

public class Person
{
    public int PersonId { get; set; }
    public  string KFUPMId { get; set; } = null!;
    public  string FirstName { get; set; }= null!;
    public  string? SecondName { get; set; }
    public  string? ThirdName { get; set; }
    public required string LastName { get; set; }
    public  DateTime DateOfBirth { get; set; }
    public int? NationalityId { get; set; }
    public List<PersonalContactInfo> PersonalContactInfos = null!;
    
}