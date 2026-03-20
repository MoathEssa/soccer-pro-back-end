namespace SoccerPro.Application.DTOs.ManagerDTOs.SearchPrams
{
    public class SearchManagersParams
    {
        public string? KFUPMId { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? NationalityId { get; set; }
        public string? TeamName { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
