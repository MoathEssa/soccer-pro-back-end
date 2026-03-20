using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Domain.Entities;

public class TeamContactInfo
{
    public int TeamContactInfoId { get; set; } // Primary Key
    public int TeamId { get; set; } // Foreign Key to Team
    public ContactType ContactType { get; set; } // e.g., Email, Phone

    public string Value { get; set; } = null!; // Contact information value

    // Navigation property
    public Team Team { get; set; } = null!;
}