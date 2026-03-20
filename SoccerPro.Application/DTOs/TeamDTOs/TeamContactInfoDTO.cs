namespace SoccerPro.Application.DTOs.TeamDTOs
{
    public class TeamContactInfoDTO
    {
        public int TeamId { get; set; }
        public int ContactType { get; set; }
        public string Value { get; set; } = null!;
    }
}
