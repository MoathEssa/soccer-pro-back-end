namespace SoccerPro.Domain.Entities.Views
{
    public class CoachView
    {
        public int CoachId { get; set; }

        // Person Info
        public int PersonId { get; set; }
        public string KFUPMId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public int? NationalityId { get; set; }

        // Team Info
        public int? TeamId { get; set; }
        public string? TeamName { get; set; }
        public DateTime? JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }

        // Computed
        public bool IsActive => LeftAt == null;
    }

}
