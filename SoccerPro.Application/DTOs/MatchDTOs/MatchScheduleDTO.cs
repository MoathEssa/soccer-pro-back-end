using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.DTOs.MatchDTOs;

public class MatchScheduleDTO
{
    public int MatchScheduleId { get; set; }
    public int TournamentId { get; set; }
    public TournamentPhase TournamentPhase { get; set; }
    public int Number { get; set; }
    public int TournamentTeamIdA { get; set; }
    public int TournamentTeamIdB { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public int FieldId { get; set; }
}