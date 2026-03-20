using System;

namespace SoccerPro.Application.DTOs.MatchDTOs
{
    public class UpcomingMatchDTO
    {
        public int MatchScheduleId { get; set; }
        public required string TeamAName { get; set; }
        public required string TeamBName { get; set; }
        public DateTime MatchDate { get; set; }
        public required string TournamentName { get; set; }
    }
}
