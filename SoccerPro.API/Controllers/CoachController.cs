using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.CoachDTOs;
using SoccerPro.Application.DTOs.TeamDTOs;
using SoccerPro.Application.Features.CoachFeature.Commands.AddCoach;
using SoccerPro.Application.Features.CoachFeature.Commands.AssignCoachIntoTeam;
using SoccerPro.Application.Features.CoachFeature.Queries.GetAllCoaches;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers;

[ApiController]
[Route("api/coaches")]
public class CoachController : AppController
{
    public CoachController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add a new coach (AddCoachDTO)",
        Description = "Creates a new coach with the provided details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<bool>>> AddCoach([FromBody] AddCoachDTO dto)
    {
        var result = await _mediator.Send(new AddCoachCommand(dto));
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("BulkInsert")]
    [SwaggerOperation(
        Summary = "Add a list of new coachs (AddCoachDTO)",
        Description = "Creates a new coach with the provided details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<bool>>> AddBulkCoach([FromBody] List<AddCoachDTO> dto)
    {
        foreach (var coach in dto)
        {
            await _mediator.Send(new AddCoachCommand(coach));
        }
        return Ok();
    }



    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all coaches",
        Description = "Retrieves a paginated list of all coaches")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<ApiResponse<List<CoachViewDTO>>>> GetAllCoaches(
    [FromQuery] string? kfupmId,
    [FromQuery] string? firstName,
    [FromQuery] string? secondName,
    [FromQuery] string? thirdName,
    [FromQuery] string? lastName,
    [FromQuery] DateTime? dateOfBirth,
    [FromQuery] int? nationalityId,
    [FromQuery] string? teamName,
    [FromQuery] bool? isActive,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var query = new GetAllCoachesQuery(
            KFUPMId: kfupmId,
            FirstName: firstName,
            SecondName: secondName,
            ThirdName: thirdName,
            LastName: lastName,
            DateOfBirth: dateOfBirth,
            NationalityId: nationalityId,
            TeamName: teamName,
            IsActive: isActive,
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
    [HttpPost("assign-coach")]
    [SwaggerOperation(Summary = "Assign coach to a team (AssignCoachIntoTeamDTO)", Description = "Assigns a coach to the specified team")]
    public async Task<ActionResult<ApiResponse<bool>>> AssignCoachToTeam([FromBody] AssignCoachIntoTeamDTO dto)
    {
        var result = await _mediator.Send(new AssignCoachIntoTeamCommand(dto));
        return StatusCode((int)result.StatusCode, result);
    }
}