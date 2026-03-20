using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RefereeDTOs;
using SoccerPro.Application.Features.RefereeFeature.Commands.AddNewReferee;
using SoccerPro.Application.Features.RefereeFeature.Commands.AssignRefereeToMatchInTournament;
using SoccerPro.Application.Features.RefereeFeature.Commands.AssignRefereeToTournament;
using SoccerPro.Application.Features.RefereeFeature.Queries.GetAllReferees;
using SoccerPro.Application.Features.RefereeFeature.Queries.GetAllRefereesInTournament;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers;

[ApiController]
[Route("api/referees")]
public class RefereeController : AppController
{
    public RefereeController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("add")]
    [SwaggerOperation(
        Summary = "Add new referee",
        Description = "Creates a new referee in the system",
        OperationId = "Referees.Add"
    )]
    public async Task<IActionResult> AddReferee([FromBody] AddRefereeDTO refereeDTO)
    {
        var command = new AddNewRefereeCommand(refereeDTO);
        var result = await _mediator.Send(command);
        return StatusCode((int)result.StatusCode, result);
    }


    [HttpPost("Bulk-Insertion")]
    [SwaggerOperation(
      Summary = "Add new referee",
      Description = "Creates a new referee in the system",
      OperationId = "Referees.Add"
  )]
    public async Task<IActionResult> AddListReferee([FromBody] List<AddRefereeDTO> refereesDTO)
    {
        foreach (var refereeDTO in refereesDTO)
        {
            var command = new AddNewRefereeCommand(refereeDTO);
            var result = await _mediator.Send(command);
        }
        return Ok();
    }

    [HttpGet("get-all")]
    [SwaggerOperation(
        Summary = "Get all referees",
        Description = "Retrieves all referees in the system",
        OperationId = "Referees.GetAll"
    )]
    public async Task<IActionResult> GetAllReferees()
    {
        var query = new GetAllRefereesQuery();
        var result = await _mediator.Send(query);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("assign-to-tournament")]
    [SwaggerOperation(
        Summary = "Assign referee to tournament",
        Description = "Assigns a referee to a specific tournament",
        OperationId = "Referees.AssignToTournament"
    )]
    public async Task<IActionResult> AssignRefereeToTournament([FromBody] AssignRefereeToTournamentCommand assignRefereeToTournamentCommand)
    {
        var result = await _mediator.Send(assignRefereeToTournamentCommand);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("assign-to-match")]
    [SwaggerOperation(
        Summary = "Assign referee to match",
        Description = "Assigns a referee to a specific match in a tournament",
        OperationId = "Referees.AssignToMatch"
    )]
    public async Task<IActionResult> AssignRefereeToMatch([FromBody] AssignRefereeToMatchInTournamentCommand assignRefereeToMatchInTournamentCommand)
    {
        var result = await _mediator.Send(assignRefereeToMatchInTournamentCommand);
        return StatusCode((int)result.StatusCode, result);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("tournament/{tournamentId}")]
    [SwaggerOperation(
        Summary = "Get all referees in a tournament",
        Description = "Retrieves a list of all referees assigned to a specific tournament."
    )]
    public async Task<ActionResult<ApiResponse<List<RefereeInTournamentDTO>>>> GetAllRefereesInTournament(int tournamentId)
    {
        var result = await _mediator.Send(new GetAllRefereesInTournamentQuery(tournamentId));
        return StatusCode((int)result.StatusCode, result);
    }
}
