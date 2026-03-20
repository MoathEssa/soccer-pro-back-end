using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SoccerPro.Application.Common.Errors;
using SoccerPro.Application.Common.Helpers;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.ManagerDTOs;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.IRepository;
using System.Net;

namespace SoccerPro.Application.Services;

public class ManagerServices : IManagerServices
{
    private readonly IManagerRepository _managerRepository;
    private readonly IMapper _mapper;
    private UserManager<User> _userManager;

    public ManagerServices(IManagerRepository managerRepository, IMapper mapper, UserManager<User> userManager)
    {
        _managerRepository = managerRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<bool>> AddManagerAsync(Manager manager, string username, string IntialPassword)
    {
        int? personId = await _managerRepository.AddManagerAsync(manager);

        if (personId == null)
            Result<bool>.Failure(Error.ValidationError("Failed to create Coach."), HttpStatusCode.BadRequest);

        var IsManagerCreated = await AuthHelpers.CreateUserWithRoleAsync(_userManager, personId.Value, username, IntialPassword, "Manager");

        return IsManagerCreated.IsSuccess
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.ValidationError("Failed to create user for Manager."), HttpStatusCode.BadRequest);

    }

    public async Task<Result<ManagerViewDTO>> GetManagerByIdAsync(int managerId)
    {

        var manager = await _managerRepository.GetManagerByIdAsync(managerId);
        if (manager is null)
            return Result<ManagerViewDTO>.Failure(Error.RecoredNotFound($"manager with id: {managerId} is not found"), System.Net.HttpStatusCode.NotFound);

        var managerDto = _mapper.Map<ManagerViewDTO>(manager);
        return Result<ManagerViewDTO>.Success(managerDto);


    }


    public async Task<Result<(List<ManagerSearchViewDTO> Managers, int TotalCount)>> SearchManagersAsync(
    string? kfupmId,
    string? firstName,
    string? secondName,
    string? thirdName,
    string? lastName,
    DateTime? dateOfBirth,
    int? nationalityId,
    string? teamName,
    int pageNumber,
    int pageSize)
    {
        var (managers, totalCount) = await _managerRepository.SearchManagersAsync(
            kfupmId,
            firstName,
            secondName,
            thirdName,
            lastName,
            dateOfBirth,
            nationalityId,
            teamName,
            pageNumber,
            pageSize
        );

        var ManagersSearchViewDTO = _mapper.Map<List<ManagerSearchViewDTO>>(managers);

        return Result<(List<ManagerSearchViewDTO>, int)>.Success((ManagersSearchViewDTO, ManagersSearchViewDTO.Count));
    }

}