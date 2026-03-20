using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RequestDTOs;
using SoccerPro.Application.Features.RequestsFeature.Commands.ProcessJoinTeamRequest;
using SoccerPro.Application.Features.RequestsFeature.Commands.RequestJoinTeamForFirstTime;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : AppController
    {
        public RequestsController(IMediator mediator) : base(mediator)
        {
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost("request-join-team-first-time")]
        [SwaggerOperation(
    Summary = "Submit a first-time join team request (RequestJoinTeamDTO)",
    Description = "Allows a user to request joining a team for the first time."
)]
        public async Task<ActionResult<ApiResponse<bool>>> RequestJoinTeam([FromBody] RequestJoinTeamDTO dto)
        {
            var result = await _mediator.Send(new RequestJoinTeamForFirstTimeCommand(dto));
            return StatusCode((int)result.StatusCode, result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("process-join-team-first-time")]
        [SwaggerOperation(
    Summary = "Process a join team request (ProcessJoinTeamRequestDTO)",
    Description = "Used by admins to approve or reject a first-time join team request."
)]
        public async Task<ActionResult<ApiResponse<bool>>> ProcessJoinTeamRequest([FromBody] ProcessJoinTeamRequestDTO dto)
        {
            var result = await _mediator.Send(new ProcessJoinTeamRequestCommand(dto));
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
