namespace SoccerPro.Domain.Entities.Views
{
    public class ManagerView
    {
        public int ManagerId { get; set; }
        public string KFUPMId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int NationalityId { get; set; }
        public string? TeamName { get; set; }
        public List<PersonalContactInfo> PersonalContactInfo { get; set; } = new();
    }


}


