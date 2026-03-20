namespace SoccerPro.Domain.Entities.Enums
{
    /// <summary>
    /// Status of a football match 100%
    /// </summary>
    public enum MatchStatus
    {
        /// <summary>Scheduled but not yet started (send value 0)</summary>
        Scheduled = 1,

        /// <summary>Match completed normally (send value 2)</summary>
        Completed = 2,

        /// <summary>Match cancelled (send value 3)</summary>
        Cancelled = 3,
    }

}
