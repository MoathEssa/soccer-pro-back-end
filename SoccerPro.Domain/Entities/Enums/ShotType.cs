namespace SoccerPro.Domain.Entities.Enums;



/// <summary>
/// Represents the type of shot taken during a match. //100%
/// </summary>
public enum ShotType
{
    /// <summary>
    /// A normal shot taken during open play (send value 1).
    /// </summary>
    Normal = 1,

    /// <summary>
    /// A penalty kick (send value 2).
    /// </summary>
    Penalty = 2,

    /// <summary>
    /// A direct shot taken from a free kick (send value 3).
    /// </summary>
    FreeKick = 3,

    /// <summary>
    /// A shot taken directly from a corner kick (send value 4).
    /// </summary>
    Corner = 4
}
