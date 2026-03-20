namespace SoccerPro.Domain.Entities.Enums;

/// <summary>
/// Types of requests in the system 100%
/// </summary>
public enum RequestType
{
    /// <summary>Request to join a team for the first time (send value 1)</summary>
    JoinTeamFirstTime = 1,

    /// <summary>Request to transfer from one team to another (send value 2)</summary>
    TransferTeam = 2,

    /// <summary>Request to leave a team (send value 3)</summary>
    LeaveTeam = 3
}
