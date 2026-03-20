using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.ManagerDTOs;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;

namespace SoccerPro.Application.Services.IServises;

public interface IManagerServices
{
    public Task<Result<bool>> AddManagerAsync(Manager manager, string username, string IntialPassword);
    Task<Result<ManagerViewDTO>> GetManagerByIdAsync(int managerId);
    Task<Result<(List<ManagerSearchViewDTO> Managers, int TotalCount)>> SearchManagersAsync(
       string? kfupmId,
       string? firstName,
       string? secondName,
       string? thirdName,
       string? lastName,
       DateTime? dateOfBirth,
       int? nationalityId,
       string? teamName,
       int pageNumber,
       int pageSize);
}