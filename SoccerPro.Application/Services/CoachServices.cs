using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SoccerPro.Application.Common.Errors;
using SoccerPro.Application.Common.Helpers;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.CoachDTOs;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.IRepository;
using System.Net;

namespace SoccerPro.Application.Services;

public class CoachServices : ICoachServices
{
    private readonly ICoachRepository _coachRepository;
    private readonly IMapper _mapper;
    private UserManager<User> _userManager;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITeamRepository _teamRepository;

    public CoachServices(ICoachRepository coachRepository, IMapper mapper, UserManager<User> userManager, ITournamentRepository tournamentRepository, ITeamRepository teamRepository)
    {
        _coachRepository = coachRepository;
        _mapper = mapper;
        _userManager = userManager;
        _tournamentRepository = tournamentRepository;
        _teamRepository = teamRepository;
    }

    public async Task<Result<bool>> AddCoachAsync(Coache coach, string username, string initialPassword)
    {
        // 1. Add the coach record to your database
        int? personId = await _coachRepository.AddCoachAsync(coach);

        if (personId == null)
            return Result<bool>.Failure(Error.ValidationError("Failed to add coach."), HttpStatusCode.BadRequest);

        var IsUserCreated = await AuthHelpers.CreateUserWithRoleAsync(_userManager, personId.Value, username, initialPassword, "Coach");

        return IsUserCreated.IsSuccess
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.ValidationError("Failed to create user for coach."), HttpStatusCode.BadRequest);

    }

    public async Task<Result<(List<CoachViewDTO> coaches, int totalCount)>> GetCoachesAsync(
      string? kfupmId = null,
      string? firstName = null,
      string? secondName = null,
      string? thirdName = null,
      string? lastName = null,
      DateTime? dateOfBirth = null,
      int? nationalityId = null,
      string? teamName = null,
      bool? isActive = null,
      int pageNumber = 1,
      int pageSize = 10)
    {
        var (coaches, totalCount) = await _coachRepository.GetCoachesAsync(
            kfupmId,
            firstName,
            secondName,
            thirdName,
            lastName,
            dateOfBirth,
            nationalityId,
            teamName,
            isActive,
            pageNumber,
            pageSize
        );

        var coachDTOs = coaches.Select(c => _mapper.Map<CoachViewDTO>(c)).ToList();

        return Result<(List<CoachViewDTO> coaches, int totalCount)>.Success((coachDTOs, totalCount));
    }

    public async Task<Result<bool>> AssignCoachToTeamAsync(CoachTeam coachTeam, int tournamentId)
    {
        // 1. Check tournament exists
        if (!await _tournamentRepository.TournamentExistsAsync(tournamentId))
        {
            return Result<bool>.Failure(Error.RecoredNotFound("Tournament not found"), HttpStatusCode.NotFound);
        }

        // 2. Check team exists
        if (!await _teamRepository.TeamExistsAsync(coachTeam.TeamId))
        {
            return Result<bool>.Failure(Error.RecoredNotFound("Team not found"), HttpStatusCode.NotFound);
        }


        // 3. Check if coach already assigned to another team in this tournament
        if (await _coachRepository.IsCoachAlreadyAssignedAsync(coachTeam.CoachId, tournamentId))
        {
            return Result<bool>.Failure(Error.ConflictError("Coach is already assigned to a team in this tournament"), HttpStatusCode.Conflict);
        }

        // 4. All validations passed → assign coach
        await _coachRepository.AssignCoachToTeamAsync(coachTeam);

        return Result<bool>.Success(true);
    }




}