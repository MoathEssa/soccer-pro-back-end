namespace SoccerPro.Domain.Entities.Views
{
    public class PlayerViolationView
    {
        public int ViolationId { get; set; }
        public string TeamName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardType { get; set; }
    }
}
