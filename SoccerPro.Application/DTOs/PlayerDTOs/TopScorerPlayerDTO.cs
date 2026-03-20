namespace SoccerPro.Application.DTOs.PlayerDTOs
{
    public class TopScorerPlayerDTO
    {
        public int PlayerTeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfGoalsPerPlayer { get; set; }
    }
}
