using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoccerPro.API.Controllers.Base;
using SoccerPro.Application.Common.ApiResponse;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.ContactInfoDTOs;
using SoccerPro.Application.DTOs.sharedDTOs;
using SoccerPro.Application.DTOs.SharedDTOs;
using SoccerPro.Application.Features.sharedFeature.Queries.FetchCountries;
using SoccerPro.Application.Features.sharedFeature.Queries.FetchDepartments;
using SoccerPro.Application.Features.SharedFeature.Queries.GetPersonalContactInfoByPersonId;
using Swashbuckle.AspNetCore.Annotations;

namespace SoccerPro.API.Controllers
{
    [ApiController]
    [Route("api/shared")]
    public class SharedController : AppController
    {
        public SharedController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("countries")]
        [SwaggerOperation(Summary = "Fetch all countries (FetchCountriesDTO)", Description = "Retrieve a list of all countries available in the system.")]
        public async Task<ActionResult<ApiResponse<List<CountryDTO>>>> FetchCountries()
        {
            var result = await _mediator.Send(new FetchCountriesQuery());
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("departments")]
        [SwaggerOperation(Summary = "Fetch all departments (DepartmentDTO)", Description = "Retrieve a list of all departments available in the system.")]
        public async Task<ActionResult<ApiResponse<List<DepartmentDTO>>>> FetchDepartments()
        {
            var result = await _mediator.Send(new FetchDepartmentsQuery());
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("personal-contact-info/{personId}")]
        [SwaggerOperation(
            Summary = "Get personal contact information by person ID",
            Description = "Retrieve all personal contact information for a specific person by their ID."
        )]
        public async Task<ActionResult<Result<List<ContactInfoDTO>>>> GetPersonalContactInfo(int personId)
        {
            var result = await _mediator.Send(new GetPersonalContactInfoByPersonIdQuery(personId));
            return StatusCode((int)result.StatusCode, result);
        }
    }
}