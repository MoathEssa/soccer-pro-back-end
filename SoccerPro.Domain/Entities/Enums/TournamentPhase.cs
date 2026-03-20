namespace SoccerPro.Domain.Entities.Enums;

/// <summary>
/// Phases of a tournament 100%
/// </summary>
public enum TournamentPhase
{
    /// <summary>Group stage phase (send value 1)</summary>
    GroupStage = 1,
    /// <summary>Quarter-finals phase (send value 2)</summary>
    QuarterFinals = 2,
    /// <summary>Semi-finals phase (send value 3)</summary>
    SemiFinals = 3,
    /// <summary>Finals phase (send value 4)</summary>
    Finals = 4,
}