namespace SoccerPro.Domain.Entities.Enums;

/// <summary>
/// Represents the role of a user in the system. 100%
/// </summary>
public enum UserRole
{
    /// <summary>
    /// System administrator with full access (send value 1).
    /// </summary>
    Admin = 1001,

    /// <summary>
    /// Coach responsible for training and managing players (send value 2).
    /// </summary>
    Coach = 1002,

    /// <summary>
    /// Registered player participating in tournaments (send value 3).
    /// </summary>
    Player = 1003,

    /// <summary>
    /// Staff member assisting with organizational tasks (send value 4).
    /// </summary>
    Staff = 1004,

    /// <summary>
    /// Guest user with limited access (send value 5).
    /// </summary>
    Guest = 1005,

    /// <summary>
    /// Team or system manager overseeing operations (send value 6).
    /// </summary>
    Manager = 1006
}
