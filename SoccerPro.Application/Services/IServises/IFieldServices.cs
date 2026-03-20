using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.SharedDTOs;

namespace SoccerPro.Application.Services.IServises;

public interface IFieldServices
{
    Task<Result<List<FieldDTO>>> GetAllFieldsAsync();
}