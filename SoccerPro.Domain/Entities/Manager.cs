namespace SoccerPro.Domain.Entities;

public class Manager
{
    public int ManagerId { get; set; } 
    public int PersonId { get; set; } 
    public Person? Person { get; set; }
}