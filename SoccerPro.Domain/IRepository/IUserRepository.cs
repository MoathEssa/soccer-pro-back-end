using SoccerPro.Domain.Entities;

namespace SoccerPro.Domain.IRepository;

public interface IUserRepository
{

    public Task<User?> GetUserByIdOrUserName(int? UserId, string? UserName);

}