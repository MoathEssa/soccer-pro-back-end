using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;
using SoccerPro.Application.Features.PlayerFeature.Commands.AddPlayer;
using SoccerPro.Application.Features.PlayerFeature.Commands.AssignPlayersIntoTeam;
using SoccerPro.Application.Features.PlayerFeature.Queries.FetchPlayerById;
using SoccerPro.Application.Features.PlayerFeature.Queries.FetchPlayers;
using SoccerPro.Application.Features.PlayerFeature.Queries.PlayerViolations;
using SoccerPro.Application.Features.PlayerFeature.Queries.TopScorerPlayer;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : AppController
    {
        public PlayerController(IMediator mediator) : base(mediator)
        {
        }



        [HttpPost()]
        [SwaggerOperation(Summary = "Create a new player (AddPlayerDTO)", Description = "Send a valid AddPlayerDTO to register a new player in the system.")]
        public async Task<ActionResult<ApiResponse<bool>>> CreatePlayer([FromBody] AddPlayerDTO playerDTO)
        {
            var result = await _mediator.Send(new AddPlayerCommand(playerDTO));
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("BulkInsert")]
        [SwaggerOperation(Summary = "Create a list of players (AddPlayerDTO)", Description = "Send a valid AddPlayerDTO to register a new player in the system.")]
        public async Task<ActionResult<ApiResponse<bool>>> CreatePlayer([FromBody] List<AddPlayerDTO> playerDTO)
        {
            foreach (var player in playerDTO)
            {
                await _mediator.Send(new AddPlayerCommand(player));
            }
            return Ok();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost("assign-player-into-Team")]
        [SwaggerOperation(Summary = "Assign players to a team (AssignPlayerIntoTeamDTO)", Description = "Assign multiple players to a team with their positions and roles")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignPlayersToTeam([FromBody] AssignPlayerIntoTeamDTO dto)
        {
            var result = await _mediator.Send(new AssignPlayerIntoTeamCommand(dto));
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet("top-scorers")]
        [ProducesResponseType(typeof(ApiResponse<List<TopScorerPlayerDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TopScorerPlayerDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTopScorers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new TopScorerPlayerQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);

            return StatusCode((int)result.StatusCode, result);
        }



        [HttpGet("player-violations")]
        [ProducesResponseType(typeof(ApiResponse<List<TopScorerPlayerDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TopScorerPlayerDTO>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTopScorers([FromQuery] int CardType, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new PlayerViolationsQuery(pageNumber, pageSize, CardType);
            var result = await _mediator.Send(query);

            return StatusCode((int)result.StatusCode, result);
        }




        [HttpGet("fetch-all")]
        [SwaggerOperation(Summary = "Fetch all players", Description = "Retrieve a list of all players available in the system.")]
        public async Task<ActionResult<ApiResponse<List<PlayerDTO>>>> FetchAllPlayers([FromQuery] int? playerId,
    [FromQuery] string? kfupmId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new FetchPlayersQuery(playerId, kfupmId, pageNumber, pageSize));
            return StatusCode((int)result.StatusCode, result);
        }




        [HttpGet("fetch/{id}")]
        [SwaggerOperation(Summary = "Fetch player by ID", Description = "Retrieve details of a player by their ID.")]
        public async Task<ActionResult<ApiResponse<PlayerDTO>>> FetchPlayerById(int id)
        {
            var result = await _mediator.Send(new FetchPlayerByIdQuery(id));
            return StatusCode((int)result.StatusCode, result);
        }


    }
}