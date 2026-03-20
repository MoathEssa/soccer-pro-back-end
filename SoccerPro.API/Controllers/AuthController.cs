using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.AuthDTOs;
using SoccerPro.Application.Features.AuthFeature.Commands.Authentication;
using SoccerPro.Application.Features.AuthFeature.Commands.RefreshToken;
using SoccerPro.Application.Features.AuthFeature.Commands.RegisterUser;
using SoccerPro.Application.Features.AuthFeature.Commands.ResetPasswordCommand;
using SoccerPro.Application.Features.AuthFeature.Queries.GetAllRoles;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class AuthController : AppController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Authenticate user (AuthenticationRequestDTO)",
            Description = "Validate credentials and return JWT plus refresh token in cookie stored."
        )]
        public async Task<ActionResult<ApiResponse<AuthenticationResponseDTO>>> Authenticate(
            [FromBody] AuthenticationRequestDTO AuthenticationRequest)
        {
            var result = await _mediator.Send(new AuthenticationCommand(AuthenticationRequest));

            if (result.Succeeded && !string.IsNullOrEmpty(result.Data?.RefreshToken))
            {
                Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            }
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("refresh")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Refresh access token ",
            Description = "Use a valid refresh token to obtain a new JWT and refresh token plus refresh token in cookie stored."
        )]
        public async Task<ActionResult<ApiResponse<AuthenticationResponseDTO>>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Refresh Token is missing or Invalid");

            var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));

            if (result.Succeeded && !string.IsNullOrEmpty(result.Data?.RefreshToken))
            {
                Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            }


            return StatusCode((int)result.StatusCode, result);
        }



        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(
                Summary = "Register user (RegsterAccountRequestDTO)",
                Description = "Create a new user account and return basic user info or tokens plus refresh token in cookie stored."
            )]
        public async Task<ActionResult<ApiResponse<AuthenticationResponseDTO>>> Register(
            [FromBody] RegsterAccountRequestDTO RegisterUserDTO)
        {
            var result = await _mediator.Send(new RegisterUserCommand(RegisterUserDTO));
            if (result.Succeeded && !string.IsNullOrEmpty(result.Data?.RefreshToken))
            {
                Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
            }
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet("roles")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Fetch all roles (RoleDTO)", Description = "Retrieve a list of all roles available in the system.")]
        public async Task<ActionResult<ApiResponse<List<RoleDTO>>>> FetchRoles()
        {
            var result = await _mediator.Send(new GetAllRolesQuery());
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("reset-password")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Reset user password (ResetPasswordDTO)", Description = "Allows a user to reset their password by providing email and new password.")]
        public async Task<ActionResult<ApiResponse<string>>> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(resetPasswordDTO));
            return StatusCode((int)result.StatusCode, result);
        }


    }
}