namespace SoccerPro.Application.DTOs.RefereeDTOs
{
    public class TournamentRefereeViewDTO
    {
        public int TournamentRefereeId { get; set; }
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }

        public int RefereeId { get; set; }
        public int PersonId { get; set; }
        public string KFUPMId { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? NationalityId { get; set; }
    }
}
