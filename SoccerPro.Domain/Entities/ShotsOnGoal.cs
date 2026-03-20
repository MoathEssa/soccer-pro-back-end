using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Domain.Entities;

public class ShotOnGoal
{
    public int ShotOnGoalId { get; set; }
    public int MatchRecoredId { get; set; }
    public int Time { get; set; }
    public int PlayerTeamId { get; set; }
    public int GoalkeeperTeamId { get; set; }
    public ShotType ShotType { get; set; }
    public bool IsGoal { get; set; }
}