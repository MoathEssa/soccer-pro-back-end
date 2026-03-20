namespace SoccerPro.Domain.Entities.Enums;

/// <summary>
/// Reasons for player substitution during a match 100%
/// </summary>
public enum SubstitutionReason
{
    /// <summary>Default reason (send value 1)</summary>
    Default = 1,
    /// <summary>Substitution due to strategy (send value 2)</summary>
    Substitution = 2,
    /// <summary>Substitution due to injury (send value 3)</summary>
    Injury = 3
}