using AutoMapper;
using SoccerPro.Application.Common.Errors;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.TournamentDTOs;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.IRepository;
using System.Net;

namespace SoccerPro.Application.Services;

public class TournamentServices : ITournamentServices
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public TournamentServices(ITournamentRepository tournamentRepository, ITeamRepository teamRepository, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<Result<bool>> AddTournamentAsync(Tournament tournament)
    {
        int insertedId = await _tournamentRepository.AddTournamentAsync(tournament);
        return Result<bool>.Success(insertedId > 0);
    }

    public async Task<Result<bool>> DeleteTournamentAsync(int tournamentId)
    {
        bool result = await _tournamentRepository.DeleteTournamentAsync(tournamentId);
        if (!result)
        {
            return Result<bool>.Failure(Error.RecoredNotFound($"Tournament with id: {tournamentId}  is not found"), System.Net.HttpStatusCode.NotFound);
        }
        return Result<bool>.Success(result);
    }
    public async Task<Result<(List<TournamentDTO> tournaments, int totalCount)>> GetAllTournamentsAsync(
        string? number = null,
        string? name = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var (tournaments, totalCount) = await _tournamentRepository.SearchTournamentsAsync(
            number: number,
            name: name,
            startDate: startDate,
            endDate: endDate,
            pageNumber: pageNumber,
            pageSize: pageSize
        );

        var tournamentDTOs = _mapper.Map<List<TournamentDTO>>(tournaments);
        return Result<(List<TournamentDTO> tournaments, int totalCount)>.Success((tournamentDTOs, totalCount));
    }

    public async Task<Result<TournamentDTO>> GetTournamentByIdAsync(int tournamentId)
    {
        var tournament = await _tournamentRepository.GetTournamentByIdAsync(tournamentId);
        if (tournament == null)
        {
            return Result<TournamentDTO>.Failure(Error.RecoredNotFound($"Tournament with id: {tournamentId} is not found"), System.Net.HttpStatusCode.NotFound);
        }
        var tournamentDTO = _mapper.Map<TournamentDTO>(tournament);
        return Result<TournamentDTO>.Success(tournamentDTO);
    }

    public async Task<Result<bool>> UpdateTournamentAsync(Tournament tournament)
    {
        var exists = await _tournamentRepository.GetTournamentByIdAsync(tournament.TournamentId);

        if (exists == null)
        {
            return Result<bool>.Failure(Error.RecoredNotFound($"Tournament with id: {tournament.TournamentId} is not found"), System.Net.HttpStatusCode.NotFound);
        }

        bool result = await _tournamentRepository.UpdateTournamentAsync(tournament);
        return Result<bool>.Success(result);
    }

    public async Task<Result<bool>> AssignTeamToTournamentAsync(int tournamentId, int teamId)
    {
        // 1. Check if the tournament exists
        var tournament = await _tournamentRepository.TournamentExistsAsync(tournamentId);
        if (!tournament)
        {
            return Result<bool>.Failure(
                Error.RecoredNotFound($"Tournament with id: {tournamentId} is not found"),
                HttpStatusCode.NotFound);
        }

        // 2. Check if the team exists
        var team = await _teamRepository.TeamExistsAsync(teamId);

        if (!team)
        {
            return Result<bool>.Failure(
                Error.RecoredNotFound($"Team with id: {teamId} is not found"),
                HttpStatusCode.NotFound);
        }

        // 3. Check if the team is already assigned to the tournament
        bool isAlreadyInTournament = await _tournamentRepository.IsTeamInTournamentAsync(teamId, tournamentId);
        if (isAlreadyInTournament)
        {
            return Result<bool>.Failure(
                Error.ValidationError("This team is already assigned to the specified tournament."),
                HttpStatusCode.Conflict);
        }

        // 4. Assign team to tournament
        await _tournamentRepository.AssignTeamToTournamentAsync(tournamentId, teamId);

        return Result<bool>.Success(true);
    }

}