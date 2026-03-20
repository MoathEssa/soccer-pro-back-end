using Microsoft.AspNetCore.Identity;

namespace SoccerPro.Domain.Entities;

public class User : IdentityUser<int>
{
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public bool MustChangePassword { get; set; } = false;

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
}