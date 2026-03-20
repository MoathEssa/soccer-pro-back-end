namespace SoccerPro.Application.DTOs.MatchDTOs
{
    public class MatchDTO
    {
        public int MatchScheduleId { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; } = string.Empty;
        public int TournamentPhase { get; set; }
        public string PhaseName { get; set; } = string.Empty;
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string TeamAName { get; set; } = string.Empty;
        public string TeamBName { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
    }
}
