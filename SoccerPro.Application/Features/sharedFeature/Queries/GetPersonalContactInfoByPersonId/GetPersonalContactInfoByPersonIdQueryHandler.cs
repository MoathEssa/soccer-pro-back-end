using AutoMapper;
using MediatR;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.Common.Errors;
using SoccerPro.Application.DTOs.ContactInfoDTOs;
using SoccerPro.Application.Services.IServises;
using System.Net;

namespace SoccerPro.Application.Features.SharedFeature.Queries.GetPersonalContactInfoByPersonId;

public class GetPersonalContactInfoByPersonIdQueryHandler : IRequestHandler<GetPersonalContactInfoByPersonIdQuery, Result<List<ContactInfoDTO>>>
{
    private readonly ISharedServices _sharedServices;
    private readonly IMapper _mapper;

    public GetPersonalContactInfoByPersonIdQueryHandler(ISharedServices sharedServices, IMapper mapper)
    {
        _sharedServices = sharedServices;
        _mapper = mapper;
    }

    public async Task<Result<List<ContactInfoDTO>>> Handle(GetPersonalContactInfoByPersonIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _sharedServices.GetPersonalContactInfoByPersonIdAsync(request.PersonId);
        
        if (!result.IsSuccess)
            return Result<List<ContactInfoDTO>>.Failure(result.Error, result.StatusCode);

        var contactInfoDTOs = _mapper.Map<List<ContactInfoDTO>>(result.Value);
        return Result<List<ContactInfoDTO>>.Success(contactInfoDTOs);
    }
}
