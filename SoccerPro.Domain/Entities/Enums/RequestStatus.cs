namespace SoccerPro.Domain.Entities.Enums;

/// <summary>
/// Status of requests in the system 100%
/// </summary>
public enum RequestStatus
{
    /// <summary>Request is pending review (send value 1)</summary>
    Pending = 1,
    /// <summary>Request was approved (send value 2)</summary>
    Approved = 2,
    /// <summary>Request was rejected (send value 3)</summary>
    Rejected = 3,
    /// <summary>Request was cancelled (send value 4)</summary>
    Cancelled = 4
}