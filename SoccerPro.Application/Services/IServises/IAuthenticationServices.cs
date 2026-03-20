using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;
namespace SoccerPro.Application.Services.IServises;

public interface IAuthenticationServices
{
    public Task<Result<AuthenticationResponseDTO>> AuthenticateUser(string userId, string password);
    public Task<Result<AuthenticationResponseDTO>> RegisterUserAsync(RegsterAccountRequestDTO registerUserDTO);
    public Task<Result<AuthenticationResponseDTO>> RefreshTokenAsync(string token);
    public Task<Result<List<RoleDTO>>> GetAllRolesAsync();
    public Task<Result<bool>> ResetPasswordDirectlyAsync(int userId, string newPassword);

}