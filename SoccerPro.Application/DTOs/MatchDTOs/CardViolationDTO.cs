namespace SoccerPro.Application.DTOs.MatchDTOs
{
    public class CardViolationDTO
    {


        public int Time { get; set; }
        public int PlayerId { get; set; }
        public int InjuredPlayerId { get; set; }
        public int TournamentRefereeId { get; set; }
        public int CardType { get; set; }
        public string? Notes { get; set; }
    }
}
