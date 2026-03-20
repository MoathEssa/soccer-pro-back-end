using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SoccerPro.Application.Common.Errors;
using SoccerPro.Application.Common.Helpers;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.RefereeDTOs;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.IRepository;
using System.Net;

namespace SoccerPro.Application.Services;

public class RefereeServices : IRefereeServices
{
    private readonly IRefereeRepository _refereeRepository;
    private readonly IMapper _mapper;
    private UserManager<User> _userManager;

    public RefereeServices(IRefereeRepository refereeRepository, IMapper mapper, UserManager<User> userManager)
    {
        _refereeRepository = refereeRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<bool>> AddRefereeAsync(Referee referee, string usename, string IntialPassword)
    {
        var personId = await _refereeRepository.AddRefereeAsync(referee);

        if (personId == null)
            return Result<bool>.Failure(Error.ValidationError("Failed to add referee."), HttpStatusCode.BadRequest);

        var IsPlayerCreated = await AuthHelpers.CreateUserWithRoleAsync(_userManager, personId.Value, usename, IntialPassword, "Referee");

        return IsPlayerCreated.IsSuccess
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.ValidationError("Failed to create user for Player."), HttpStatusCode.BadRequest);
    }

    public async Task<Result<List<RefereeViewDTO>>> GetAllRefereesAsync()
    {
        var referees = await _refereeRepository.GetAllRefereesAsync();
        var refereeDTOs = _mapper.Map<List<RefereeViewDTO>>(referees);
        return Result<List<RefereeViewDTO>>.Success(refereeDTOs);
    }

    public async Task<Result<bool>> AssignRefereeToTournamentAsync(int refereeId, int tournamentId)
    {
        var tournamentReferee = new TournamentReferee
        {
            RefereeId = refereeId,
            TournamentId = tournamentId
        };

        bool flag = await _refereeRepository.IsRefereeInTournamentAsync(refereeId, tournamentId);
        if (flag)
        {
            return Result<bool>.Failure(
                Error.ValidationError($"The referee with ID {refereeId} is already assigned to tournament ID {tournamentId}."),
                HttpStatusCode.BadRequest
            );
        }


        var result = await _refereeRepository.AssignRefereeToTournamentAsync(tournamentId, refereeId);

        if (!result)
        {
            return Result<bool>.Failure(
                Error.ValidationError($"Failed to assign referee {refereeId} to tournament {tournamentId}"),
                HttpStatusCode.BadRequest
            );
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> AssignRefereeToMatchAsync(int matchScheduleId, int tournamentRefereeId)
    {

        bool flag = await _refereeRepository.IsRefereeInSameMatchAsync(matchScheduleId, tournamentRefereeId);
        if (flag)
        {
            return Result<bool>.Failure(
                Error.ValidationError("This referee is already assigned to the specified match."),
                HttpStatusCode.BadRequest
            );
        }

        await _refereeRepository.AssignTournamentRefereeToMatchAsync(matchScheduleId, tournamentRefereeId);



        return Result<bool>.Success(true);
    }

    public async Task<Result<List<TournamentRefereeViewDTO>>> GetRefereesInTournamentAsync(int tournamentId)
    {

        var referees = await _refereeRepository.GetRefereesInTournamentAsync(tournamentId);

        var refereeDTOs = _mapper.Map<List<TournamentRefereeViewDTO>>(referees);

        return Result<List<TournamentRefereeViewDTO>>.Success(refereeDTOs);

    }
}
