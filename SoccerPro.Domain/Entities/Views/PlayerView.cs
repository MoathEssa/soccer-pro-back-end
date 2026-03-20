namespace SoccerPro.Domain.Entities.Views;    

public class PlayerView{
    public int PlayerId { get; set; }
    public int PersonId { get; set; }
    public int PlayerType { get; set; }       
    public int PlayerStatus { get; set; }     
    public int DepartmentId { get; set; }
    public string KFUPMId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? SecondName { get; set; }
    public string? ThirdName { get; set; }
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public int NationalityId { get; set; }

}