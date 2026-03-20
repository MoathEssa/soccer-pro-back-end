namespace SoccerPro.Domain.Entities.Enums;

/// <summary>
/// Status of a player in the system 100%
/// </summary>
public enum PlayerStatus
{
    /// <summary>Allowed to play (send value 1)</summary>
    AllowedToPlay = 1,
    /// <summary>Suspended for two matches (send value 2)</summary>
    SuspendedTwoMatches = 2,
    /// <summary>Suspended for one match (send value 3)</summary>
    SuspendedOneMatch = 3,
    /// <summary>Injured (send value 4)</summary>
    Injured = 3,
    /// <summary>Not allowed to play (send value 5)</summary>
    NotAllowedToPlay = 4,
}