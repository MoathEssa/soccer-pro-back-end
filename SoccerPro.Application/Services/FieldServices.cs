using AutoMapper;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.SharedDTOs;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.IRepository;

namespace SoccerPro.Application.Services;

public class FieldServices : IFieldServices
{
    private readonly IFieldRepository _fieldRepository;
    private readonly IMapper _mapper;

    public FieldServices(IFieldRepository fieldRepository, IMapper mapper)
    {
        _fieldRepository = fieldRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<FieldDTO>>> GetAllFieldsAsync()
    {
        var fields = await _fieldRepository.GetAllFieldsAsync();
        var fieldDTOs = _mapper.Map<List<FieldDTO>>(fields);
        return Result<List<FieldDTO>>.Success(fieldDTOs);
    }
}