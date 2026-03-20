namespace SoccerPro.Application.DTOs.AuthDTOs
{
    public class ResetPasswordDTO
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
