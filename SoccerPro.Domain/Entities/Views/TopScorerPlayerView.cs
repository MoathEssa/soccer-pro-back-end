namespace SoccerPro.Domain.Entities.Views
{
    public class TopScorerPlayerView
    {
        public int PlayerTeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfGoalsPerPlayer { get; set; }
    }
}
