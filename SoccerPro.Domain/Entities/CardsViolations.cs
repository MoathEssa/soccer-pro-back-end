namespace SoccerPro.Domain.Entities;

using SoccerPro.Domain.Entities.Enums;

public class CardViolation
{
    public int TournamentRefereeId { set; get; }
    public int ViolationId { get; set; }
    public int MatchRecordId { get; set; }
    public int Time { get; set; }
    public int PlayerId { get; set; }
    public int InjuredPlayerId { get; set; }
    public CardType CardType { get; set; } // e.g., Red, Yellow
    public string? Notes { get; set; }
}