namespace SoccerPro.Domain.Entities;

public class Leader
{
    public int LeaderId { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
}