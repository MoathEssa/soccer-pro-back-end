using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.MatchDTOs;
using SoccerPro.Application.Features.MatchFeature.Commands.AddMatchResult;
using SoccerPro.Application.Features.MatchFeature.Commands.ScheduleMatch;
using SoccerPro.Application.Features.MatchFeature.Queries.GetAllScheduledMatches;
using SoccerPro.Application.Features.MatchFeature.Queries.GetUpcomingMatchesByTeam;
using SoccerPro.Domain.Entities.Views;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace SoccerPro.API.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchController : AppController
{
    public MatchController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("schedule")]
    [SwaggerOperation(
        Summary = "Schedule a match",
        Description = "Creates a new match schedule between two teams in a tournament")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> ScheduleMatch([FromBody] ScheduleMatchCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all scheduled matches",
        Description = "Returns a paginated list of all scheduled matches with optional filtering by tournament, phase, team names, field, and date")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<List<MatchView>>>> GetAllScheduledMatches(
        [FromQuery] int? tournamentId,
        [FromQuery] int? tournamentPhase,
        [FromQuery] string? teamAName,
        [FromQuery] string? teamBName,
        [FromQuery] string? fieldName,
        [FromQuery] DateTime? matchDate,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetAllScheduledMatchesQuery(
            TournamentId: tournamentId,
            TournamentPhase: tournamentPhase,
            TeamAName: teamAName,
            TeamBName: teamBName,
            FieldName: fieldName,
            MatchDate: matchDate,
            PageNumber: pageNumber,
            PageSize: pageSize
        );

        var result = await _mediator.Send(query);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("results")]
    [SwaggerOperation(
        Summary = "Record match results (AddMatchResultDTO)",
        Description = "Records the results of a completed match including goals, acquisition rates, and shots on goal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> AddMatchResult(
    [FromBody] List<AddMatchRecordDTO> AddMatchResultADTO)
    {
        foreach (var matchResult in AddMatchResultADTO)
        {
            var result = await _mediator.Send(new AddMatchResultCommand(matchResult));

            if (!result.Succeeded) // Assuming ApiResponse has this property
            {
                return StatusCode((int)result.StatusCode, result);
            }
        }

        return Ok(new ApiResponse<bool>
        {
            Succeeded = true,
            Message = "All match records inserted successfully.",
            Data = true,
            StatusCode = HttpStatusCode.OK
        });
    }


    [HttpGet("upcoming-matches")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UpcomingMatchDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UpcomingMatchDTO>>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UpcomingMatchDTO>>>> GetUpcomingMatchesByTeam(
        [FromQuery] string? teamName,
        [FromQuery] string? tournamentName = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetUpcomingMatchesByTeamQuery(teamName, tournamentName, fromDate, toDate, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}