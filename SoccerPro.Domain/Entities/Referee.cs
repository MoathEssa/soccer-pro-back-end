namespace SoccerPro.Domain.Entities;

public class Referee
{
    public int RefereeId { get; set; } // Primary Key
    public int PersonId { get; set; } // Foreign Key to Person

    // Navigation property
    public Person Person { get; set; } = null!;
}