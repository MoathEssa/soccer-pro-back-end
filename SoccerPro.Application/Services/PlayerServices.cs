using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Namespace.SoccerPro.Domain.IRepository;
using SoccerPro.Application.Common.Errors;
using SoccerPro.Application.Common.Helpers;
using SoccerPro.Application.Common.ResultPattern;
using SoccerPro.Application.DTOs.PlayerDTOs;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;
using SoccerPro.Domain.IRepository;
using System.Net;

namespace Namespace.SoccerPro.Application.Services.PlayerServices;

public class PlayerServices : IPlayerServices
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;
    private UserManager<User> _userManager;

    public PlayerServices(IPlayerRepository playerRepository, ITournamentRepository tournamentRepository, ITeamRepository teamRepository, IMapper mapper, UserManager<User> userManager)
    {
        _playerRepository = playerRepository;
        _tournamentRepository = tournamentRepository;
        _teamRepository = teamRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<bool>> AddPlayerAsync(Player player, string username, string IntialPassword)
    {
        int? personId = await _playerRepository.AddPlayerAsync(player);

        if (personId == null)
            return Result<bool>.Failure(Error.ValidationError("Failed to add player."), HttpStatusCode.BadRequest);

        var IsPlayerCreated = await AuthHelpers.CreateUserWithRoleAsync(_userManager, personId.Value, username, IntialPassword, "Player");

        return IsPlayerCreated.IsSuccess
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.ValidationError("Failed to create user for Player."), HttpStatusCode.BadRequest);
    }

    public async Task<Result<(List<PlayerDTO> playerDTOs, int totalCount)>> GetAllPlayersAsync(
      int? playerId,
      string? kfupmId,
      int pageNumber = 1,
      int pageSize = 10)
    {
        (List<PlayerView> playerViews, int totalCount) = await _playerRepository.GetAllPlayersAsync(playerId, kfupmId, pageNumber, pageSize);

        var playerDTOs = _mapper.Map<List<PlayerDTO>>(playerViews);

        return Result<(List<PlayerDTO> playerDTOs, int totalCount)>.Success((playerDTOs, totalCount));
    }


    public async Task<Result<PlayerDTO>> GetPlayerByIdAsync(int playerId)
    {
        var player = await _playerRepository.GetPlayerByIdAsync(playerId);
        var playerDto = _mapper.Map<PlayerDTO>(player);
        if (player == null)
        {
            return Result<PlayerDTO>.Failure(Error.RecoredNotFound($"Player with id: {playerId} is not found"), System.Net.HttpStatusCode.NotFound);
        }

        return Result<PlayerDTO>.Success(playerDto);
    }

    public async Task<Result<bool>> AssignPlayerToTeamAsync(PlayerTeam playerTeam, int tournamentId)
    {
        // 1. Check if tournament exists
        if (!await _tournamentRepository.TournamentExistsAsync(tournamentId))
        {
            return Result<bool>.Failure(
                Error.RecoredNotFound("Tournament not found."),
                HttpStatusCode.NotFound);
        }

        // 2. Check if team exists
        if (!await _teamRepository.TeamExistsAsync(playerTeam.TeamId))
        {
            return Result<bool>.Failure(
                Error.RecoredNotFound("Team not found."),
                HttpStatusCode.NotFound);
        }

        // 3. Check if team is part of the tournament
        if (!await _teamRepository.IsTeamInTournamentAsync(playerTeam.TeamId, tournamentId))
        {
            return Result<bool>.Failure(
                Error.ValidationError("Team is not participating in the specified tournament."),
                HttpStatusCode.BadRequest);
        }

        // 4. Check if player is already assigned in this tournament
        if (await _playerRepository.IsPlayerAlreadyAssignedAsync(playerTeam.PlayerId, tournamentId))
        {
            return Result<bool>.Failure(
                Error.ConflictError("Player is already assigned to another team in this tournament."),
                HttpStatusCode.Conflict);
        }

        // 5. All checks passed — assign player
        await _playerRepository.AssignPlayerToTeamAsync(playerTeam);

        return Result<bool>.Success(true);
    }

    public async Task<Result<List<TopScorerPlayerView>>> GetTopScorersAsync(int pageNumber, int pageSize)
    {

        var topScorers = await _playerRepository.GetTopScorersAsync(pageNumber, pageSize);
        return Result<List<TopScorerPlayerView>>.Success(topScorers);


    }

    public async Task<Result<List<PlayerViolationView>>> GetPlayerViolationsAsync(int pageNumber, int pageSize, int? cardType)
    {
        var violations = await _playerRepository.GetPlayerViolationsAsync(pageNumber, pageSize, cardType);


        return Result<List<PlayerViolationView>>.Success(violations);

    }
}