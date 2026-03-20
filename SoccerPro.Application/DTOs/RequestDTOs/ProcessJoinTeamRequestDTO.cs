namespace SoccerPro.Application.DTOs.RequestDTOs
{
    public class ProcessJoinTeamRequestDTO
    {
        public int RequestId { get; set; }
        public int ProcessorUserId { get; set; }
        public int RequestStatus { get; set; } // e.g., 2 = Approved, 3 = Rejected
        public int PlayerStatus { get; set; }  // e.g., 1 = Active
    }
}
