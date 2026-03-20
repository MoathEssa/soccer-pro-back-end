using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TournamentDTOs;
using SoccerPro.Application.Features.TournamentFeature.Commands.AddTournament;
using SoccerPro.Application.Features.TournamentFeature.Commands.AssignTeamsInTournament;
using SoccerPro.Application.Features.TournamentFeature.Commands.DeleteTournament;
using SoccerPro.Application.Features.TournamentFeature.Commands.UpdateTournament;
using SoccerPro.Application.Features.TournamentFeature.Queries.FetchTournamentById;
using SoccerPro.Application.Features.TournamentFeature.Queries.FetchTournaments;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers
{

    [ApiController]
    [Route("api/Tournament")]
    public class TournamentController : AppController
    {
        public TournamentController(IMediator mediator) : base(mediator)
        {
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost]
        [SwaggerOperation(Summary = "Create a tournament (AddTournamentDTO)", Description = "Creates a new tournament.")]
        public async Task<ActionResult<ApiResponse<TournamentDTO>>> Create([FromBody] AddTournamentDTO addTournamentDTO)
        {
            var result = await _mediator.Send(new AddTournamentCommand(addTournamentDTO));
            return StatusCode((int)result.StatusCode, result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all tournaments (TournamentDTO)", Description = "Returns a list of all tournaments.")]
        public async Task<ActionResult<ApiResponse<List<TournamentDTO>>>> GetAll(
            [FromQuery] string? tournamentNumber,
            [FromQuery] string? tournamentName,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var result = await _mediator.Send(new FetchTournamentsQuery(tournamentNumber, tournamentName, startDate, endDate, pageNumber, pageSize));

            return StatusCode((int)result.StatusCode, result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get tournament by it's Id (TournamentDTO)", Description = "Returns a single tournament by its ID.")]
        public async Task<ActionResult<ApiResponse<TournamentDTO>>> GetById(int id)
        {
            var result = await _mediator.Send(new FetchTournamentByIdQuery(id));
            return StatusCode((int)result.StatusCode, result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut()]
        [SwaggerOperation(Summary = "Update a tournament (UpdateTournamentDTO)", Description = "Updates an existing tournament.")]
        public async Task<ActionResult<ApiResponse<TournamentDTO>>> Update([FromBody] UpdateTournamentDTO updateTournamentDTO)
        {
            var result = await _mediator.Send(new UpdateTournamentCommand(updateTournamentDTO));
            return StatusCode((int)result.StatusCode, result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a tournament", Description = "Deletes a tournament by ID.")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTournamentCommand(id));
            return StatusCode((int)result.StatusCode, result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost("assign-team")]
        [SwaggerOperation(Summary = "Assign team to a tournament (AssignTeamInTournamentDTO)", Description = "Assigns team to a tournament.")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignTeams([FromBody] AssignTeamInTournamentDTO assignTeamsDTO)
        {
            var result = await _mediator.Send(new AssignTeamInTournamentCommand(assignTeamsDTO));
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
