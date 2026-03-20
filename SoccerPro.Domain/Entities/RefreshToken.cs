namespace SoccerPro.Domain.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public string Token { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = new User();

        public DateTime ExpiryTime { get; set; }

        public bool IsRevoked { get; set; } = false;
        public bool IsExpired => DateTime.UtcNow > ExpiryTime;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
