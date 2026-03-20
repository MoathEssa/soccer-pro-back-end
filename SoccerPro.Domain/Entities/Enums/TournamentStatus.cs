namespace SoccerPro.Domain.Entities.Enums
{

    /// <summary>
    /// Represents the status of a tournament.
    /// </summary>
    public enum TournamentStatus
    {
        /// <summary>
        /// The tournament is scheduled to occur in the future.
        /// </summary>
        Upcoming = 1,

        /// <summary>
        /// The tournament is currently in progress.
        /// </summary>
        Ongoing = 2,

        /// <summary>
        /// The tournament has been completed.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// The tournament has been canceled.
        /// </summary>
        Canceled = 4
    }
}


