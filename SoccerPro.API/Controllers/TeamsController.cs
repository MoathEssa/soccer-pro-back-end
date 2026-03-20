using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TeamDTOs;
using SoccerPro.Application.Features.TeamsFeature.Commands.AddTeam;
using SoccerPro.Application.Features.TeamsFeature.Commands.DeleteTeam;
using SoccerPro.Application.Features.TeamsFeature.Commands.UpdateTeam;
using SoccerPro.Application.Features.TeamsFeature.Queries.FetchTeams;
using SoccerPro.Application.Features.TeamsFeature.Queries.FetchTeamsByTournament;
using SoccerPro.Application.Features.TeamsFeature.Queries.GetTeamInfoById;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers;

[ApiController]
[Route("api/teams")]
public class TeamsController : AppController
{
    public TeamsController(IMediator mediator) : base(mediator)
    {
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a team (AddTeamDTO)", Description = "Creates a new team with the provided details.")]
    public async Task<ActionResult<ApiResponse<bool>>> CreateTeam([FromBody] AddTeamDTO teamDTO)
    {
        var result = await _mediator.Send(new AddTeamCommand(teamDTO));
        return StatusCode((int)result.StatusCode, result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPut]
    [SwaggerOperation(Summary = "Update a team (UpdateTeamDTO)", Description = "Updates an existing team with the provided details.")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateTeam([FromBody] UpdateTeamDTO teamDTO)
    {
        var result = await _mediator.Send(new UpdateTeamCommand(teamDTO));
        return StatusCode((int)result.StatusCode, result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("{teamId}")]
    [SwaggerOperation(Summary = "Delete a team", Description = "Deletes the specified team.")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteTeam(int teamId)
    {
        var result = await _mediator.Send(new DeleteTeamCommand(teamId));
        return StatusCode((int)result.StatusCode, result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet]
    [SwaggerOperation(
      Summary = "Search teams (TeamDTO)",
      Description = "Retrieves a paginated, filtered list of teams with optional search by name, manager, and other properties.")]
    public async Task<ActionResult<ApiResponse<(List<TeamDTO> Teams, int TotalCount)>>> GetTeams(
      [FromQuery] string? name = null,
      [FromQuery] string? address = null,
      [FromQuery] string? website = null,
      [FromQuery] int? numberOfPlayers = null,
      [FromQuery] int? managerId = null,
      [FromQuery] string? managerFirstName = null,
      [FromQuery] string? managerLastName = null,
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new FetchTeamsQuery(
            name,
            address,
            website,
            numberOfPlayers,
            managerId,
            managerFirstName,
            managerLastName,
            pageNumber,
            pageSize
        ));

        return StatusCode((int)result.StatusCode, result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("by-tournamentId/{tournamentId}")]
    [SwaggerOperation(
        Summary = "Get teams by tournament",
        Description = "Retrieves a paginated list of teams participating in a specific tournament")]
    public async Task<ActionResult<ApiResponse<List<TeamTournamentViewDTO>>>> GetTeamsByTournament(
        int tournamentId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new FetchTeamsByTournamentQuery(
            TournamentId: tournamentId,
            PageNumber: pageNumber,
            PageSize: pageSize
        );

        var result = await _mediator.Send(query);
        return StatusCode((int)result.StatusCode, result);
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("getTeamById")]
    [SwaggerOperation(
        Summary = "Get team by Id",
        Description = "Get team info by TeamId")]
    public async Task<ActionResult<ApiResponse<TeamViewDTO>>> GetTeamsById(
        [FromQuery] int teamId
        )
    {
        var query = new GetTeamInfoByIdQuery(
            teamId
        );

        var result = await _mediator.Send(query);
        return StatusCode((int)result.StatusCode, result);
    }
}