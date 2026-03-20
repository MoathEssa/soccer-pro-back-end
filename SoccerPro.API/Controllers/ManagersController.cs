using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.DTOs.ManagerDTOs;
using SoccerPro.Application.DTOs.ManagerDTOs.SearchPrams;
using SoccerPro.Application.Features.ManagersFeature.Commands.AddManager;
using SoccerPro.Application.Features.ManagersFeature.Queries.GetManager;
using SoccerPro.Application.Features.ManagersFeature.Queries.SearchManagers;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers;

public class ManagersController : AppController
{
    public ManagersController(IMediator mediator) : base(mediator) { }

    [HttpPost("add-manager")]
    [SwaggerOperation(
        Summary = "Add a new manager (AddManagerDTO)",
        Description = "Creates a new manager with the provided details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddManager([FromBody] AddManagerDTO dto)
    {
        var result = await _mediator.Send(new AddManagerCommand(dto));
        return StatusCode((int)result.StatusCode, result);

    }

    [HttpPost("Bulk-insert-managers")]
    [SwaggerOperation(
        Summary = "Add a new list of managers (AddManagerDTO)",
        Description = "Creates a new manager with the provided details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBulkManager([FromBody] List<AddManagerDTO> dto)
    {

        foreach (var manager in dto)
        {
            await _mediator.Send(new AddManagerCommand(manager));
        }

        return Ok();

    }

    [HttpGet("get/{id:int}")]
    [SwaggerOperation(
        Summary = "Get manager by ID (ManagerDTO)",
        Description = "Retrieves a manager's details by their ID")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetManager([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetManagerQuery(id));
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("search-managers")]
    [SwaggerOperation(
    Summary = "Search for managers (ManagerDTO)",
    Description = "Returns a filtered, paginated list of managers based on provided criteria.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchManagers([FromQuery] SearchManagersParams SearchManagersParams)
    {
        var result = await _mediator.Send(new SearchManagersQuery(SearchManagersParams.KFUPMId, SearchManagersParams.FirstName, SearchManagersParams.SecondName, SearchManagersParams.ThirdName, SearchManagersParams.LastName, SearchManagersParams.DateOfBirth, SearchManagersParams.NationalityId, SearchManagersParams.TeamName, SearchManagersParams.PageNumber, SearchManagersParams.PageSize));
        return StatusCode((int)result.StatusCode, result);
    }
}