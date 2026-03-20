using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SoccerPro.Application.Common.Errors;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SoccerPro.Application.Common.Helpers
{
    public static class AuthHelpers
    {
        public static string GenerateJwtToken(User user, JwtSettings jwtSettings, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                        new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
                    };

            // Add roles to claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = jwtSettings.Audience,
                Issuer = jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public static async Task<Result<bool>> CreateUserWithRoleAsync(UserManager<User> _userManager, int personId, string username, string initialPassword, string role)
        {
            var user = new User
            {
                UserName = username,
                Email = username,
                MustChangePassword = true,
                PersonId = personId
            };

            var userResult = await _userManager.CreateAsync(user, initialPassword);
            if (!userResult.Succeeded)
            {
                var errors = string.Join("; ", userResult.Errors.Select(e => e.Description));
                return Result<bool>.Failure(Error.ValidationError($"Failed to create user: {errors}"), HttpStatusCode.BadRequest);
            }

            var roleAssignResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleAssignResult.Succeeded)
            {
                var errors = string.Join("; ", roleAssignResult.Errors.Select(e => e.Description));
                return Result<bool>.Failure(Error.ValidationError($"User created but role assignment failed: {errors}"), HttpStatusCode.BadRequest);
            }

            return Result<bool>.Success(true);
        }



        public static string GetRefreshToken()
        {
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

    }
}
