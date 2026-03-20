namespace SoccerPro.Application.DTOs.PlayerDTOs
{
    public class PlayerViolationDTO
    {
        public int ViolationId { get; set; }
        public string TeamName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardType { get; set; }
    }
}
