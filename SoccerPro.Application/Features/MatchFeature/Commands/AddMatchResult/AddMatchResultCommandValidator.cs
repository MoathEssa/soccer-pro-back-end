using FluentValidation;
using Namespace.SoccerPro.Domain.IRepository;
using SoccerPro.Application.DTOs.MatchDTOs;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.IRepository;

namespace SoccerPro.Application.Features.MatchFeature.Commands.AddMatchResult;

public class AddMatchResultCommandValidator : AbstractValidator<AddMatchResultCommand>
{
    private readonly IMatchRepository _matchRepo;
    private readonly IPlayerRepository _playerRepository;

    private bool TryGetMatch(IValidationContext validationContext, out MatchSchedule match)
    {
        match = null;

        if (!validationContext.RootContextData.TryGetValue("match", out var matchObj) || matchObj is not MatchSchedule validMatch)
            return false;

        match = validMatch;
        return true;
    }


    public AddMatchResultCommandValidator(IMatchRepository matchRepo, IPlayerRepository playerRepository)
    {
        _matchRepo = matchRepo;
        _playerRepository = playerRepository;

        // Basic validation for the command
        RuleFor(x => x.Dto.MatchScheduleId)
            .GreaterThan(0)
            .WithMessage("MatchScheduleId must be greater than 0");

        // Team A validation
        RuleFor(x => x.Dto.TeamRecord)
            .NotNull()
            .WithMessage("Team A result is required")
            .SetValidator(new TeamMatchResultDTOValidator());


        // Validate shots on goal for Team A
        RuleForEach(x => x.Dto.TeamRecord.ShotsOnGoal)
            .SetValidator(new ShotOnGoalDTOValidator());



        //// Step 1: Fetch match once and cache it
        //RuleFor(x => x)
        //    .MustAsync(async (command, cancellation) =>
        //    {
        //        var match = await matchRepo.GetMatchScheduleByIdAsync(command.Dto.MatchScheduleId);
        //        if (match == null) return false;

        //        (command as IValidationContext)?.RootContextData.TryAdd("match", match);
        //        return true;
        //    })
        //    .WithMessage(x => $"Match with id: {x.Dto.MatchScheduleId} is not found");

        //// Step 2: Ensure match is scheduled
        //RuleFor(x => x)
        //    .Must((command, _, context) =>
        //    {
        //        if (!TryGetMatch(context, out MatchSchedule match))
        //            return true;

        //        return match.MatchStatus == MatchStatus.Scheduled;
        //    })
        //    .WithMessage("Match is already recorded.");

        //// Step 3: Ensure teams are not the same
        //RuleFor(x => x)
        //    .Must(x => x.Dto.TeamARecord.TournamentTeamId != x.Dto.TeamBRecord.TournamentTeamId)
        //    .WithMessage("Team A and Team B cannot be the same team.");

        //// Step 4: Team A belongs to the match
        //RuleFor(x => x.Dto.TeamARecord.TournamentTeamId)
        //    .Must((command, TournamentTeamId, context) =>
        //    {
        //        if (!TryGetMatch(context, out MatchSchedule match))
        //            return true;

        //        return TournamentTeamId == match.TournamentTeamIdA || TournamentTeamId == match.TournamentTeamIdB;
        //    })
        //    .WithMessage("Team A is not one of the scheduled teams.");

        //// Step 5: Team B belongs to the match
        //RuleFor(x => x.Dto.TeamBRecord.TournamentTeamId)
        //    .Must((command, teamId, context) =>
        //    {
        //        if (!TryGetMatch(context, out MatchSchedule match))
        //            return true;

        //        return teamId == match.TournamentTeamIdA || teamId == match.TournamentTeamIdB;
        //    })
        //    .WithMessage("Team B is not one of the scheduled teams.");

        //// Step 6: Goals for/against match
        //RuleFor(x => x)
        //    .Must(x => x.Dto.TeamARecord.GoalsFor == x.Dto.TeamBRecord.GoalAgainst)
        //    .WithMessage("Team A's GoalsFor must match Team B's GoalAgainst");

        //RuleFor(x => x)
        //    .Must(x => x.Dto.TeamBRecord.GoalsFor == x.Dto.TeamARecord.GoalAgainst)
        //    .WithMessage("Team B's GoalsFor must match Team A's GoalAgainst");

        //// Step 7: Validate best players if match is completed
        //RuleFor(x => x)
        //    .CustomAsync(async (command, context, cancellation) =>
        //    {
        //        if (command.Dto.UpdatedMatchStatus != MatchStatus.Completed)
        //            return;

        //        if (command.Dto.TeamARecord.BestPlayer.HasValue && command.Dto.TeamBRecord.BestPlayer.HasValue)
        //        {
        //            context.AddFailure("BestPlayer", "Only one team can have a BestPlayer in a match.");
        //            return;
        //        }


        //        if (!TryGetMatch(context, out MatchSchedule match))
        //            return;


        //        // Team A BestPlayer
        //        if (command.Dto.TeamARecord.BestPlayer.HasValue)
        //        {
        //            var isValid = await _playerRepository.IsPlayerAlreadyAssignedAsync(
        //                command.Dto.TeamARecord.BestPlayer.Value,
        //                match.TournamentId);

        //            if (!isValid)
        //            {
        //                context.AddFailure("TeamARecord.BestPlayer", $"Player {command.Dto.TeamARecord.BestPlayer.Value} is not in the tournament");
        //            }
        //            return;
        //        }

        //        // Team B BestPlayer
        //        if (command.Dto.TeamBRecord.BestPlayer.HasValue)
        //        {
        //            var isValid = await _playerRepository.IsPlayerAlreadyAssignedAsync(
        //                command.Dto.TeamBRecord.BestPlayer.Value,
        //                match.TournamentId);

        //            if (!isValid)
        //            {
        //                context.AddFailure("TeamBRecord.BestPlayer", $"Player {command.Dto.TeamBRecord.BestPlayer.Value} is not in the tournament");
        //            }

        //        }
        //    });

        //// Step 8: Validate All Players in Team A are from the correct team and tournament
        //RuleFor(x => x)
        //    .CustomAsync(async (command, context, cancellation) =>
        //    {
        //        if (!TryGetMatch(context, out MatchSchedule match))
        //            return;

        //        var allPlayerIds = new List<int>();

        //        // Players who made shots for Team A
        //        if (command.Dto.TeamARecord?.ShotsOnGoal != null)
        //            allPlayerIds.AddRange(command.Dto.TeamARecord.ShotsOnGoal.Select(s => s.PlayerTeamId));

        //        // Players substituted into the game for Team A
        //        if (command.Dto.TeamARecord?.matchSubstitutionDTOs != null)
        //            allPlayerIds.AddRange(command.Dto.TeamARecord.matchSubstitutionDTOs.Select(s => s.PlayerInTeamId));

        //        // Players who received cards for Team A
        //        if (command.Dto.TeamARecord?.CardsViolations != null)
        //            allPlayerIds.AddRange(command.Dto.TeamARecord.CardsViolations.Select(c => c.PlayerId));



        //        // Add goalkeepers from Team B’s shots (they're Team A's goalkeepers)
        //        if (command.Dto.TeamBRecord?.ShotsOnGoal != null)
        //            allPlayerIds.AddRange(command.Dto.TeamBRecord.ShotsOnGoal.Select(s => s.GoalkeeperTeamId));

        //        // Add Injured Players from Team B’s shots (they're Team A's Players)
        //        if (command.Dto.TeamBRecord?.CardsViolations != null)
        //            allPlayerIds.AddRange(command.Dto.TeamBRecord.CardsViolations.Select(s => s.InjuredPlayerTeamId));

        //        // Remove duplicates
        //        allPlayerIds = allPlayerIds
        //            .Where(id => id > 0)
        //            .Distinct()
        //            .ToList();

        //        if (allPlayerIds.Count == 0)
        //            return;

        //        var isValid = await _matchRepo.ValidatePlayersInTeamInTournamentAsync(
        //            match.TournamentId,
        //            match.TournamentTeamIdA,
        //            allPlayerIds
        //        );

        //        if (!isValid)
        //        {
        //            context.AddFailure("TeamAResult", "Some players listed for Team A are not valid members of the team in this tournament.");
        //        }
        //    });

        //// Step 9: Validate All Players in Team B are from the correct team and tournament
        //RuleFor(x => x)
        //    .CustomAsync(async (command, context, cancellation) =>
        //    {
        //        if (!TryGetMatch(context, out MatchSchedule match))
        //            return;

        //        var allPlayerIds = new List<int>();

        //        // Players who made shots for Team B
        //        if (command.Dto.TeamBRecord?.ShotsOnGoal != null)
        //            allPlayerIds.AddRange(command.Dto.TeamBRecord.ShotsOnGoal.Select(s => s.PlayerTeamId));

        //        // Players substituted into the game for Team B
        //        if (command.Dto.TeamBRecord?.matchSubstitutionDTOs != null)
        //            allPlayerIds.AddRange(command.Dto.TeamBRecord.matchSubstitutionDTOs.Select(s => s.PlayerInTeamId));

        //        // Players who received cards for Team B
        //        if (command.Dto.TeamBRecord?.CardsViolations != null)
        //            allPlayerIds.AddRange(command.Dto.TeamBRecord.CardsViolations.Select(c => c.PlayerId));

        //        // Add goalkeepers from Team A’s shots (they're Team B's goalkeepers)
        //        if (command.Dto.TeamARecord?.ShotsOnGoal != null)
        //            allPlayerIds.AddRange(command.Dto.TeamARecord.ShotsOnGoal.Select(s => s.GoalkeeperTeamId));

        //        // Add Injured Players from Team A’s shots (they're Team B's Players)
        //        if (command.Dto.TeamARecord?.CardsViolations != null)
        //            allPlayerIds.AddRange(command.Dto.TeamARecord.CardsViolations.Select(s => s.InjuredPlayerTeamId));


        //        // Remove duplicates and invalid IDs
        //        allPlayerIds = allPlayerIds
        //            .Where(id => id > 0)
        //            .Distinct()
        //            .ToList();

        //        if (allPlayerIds.Count == 0)
        //            return;

        //        var isValid = await _matchRepo.ValidatePlayersInTeamInTournamentAsync(
        //            match.TournamentId,
        //            match.TournamentTeamIdB,
        //            allPlayerIds
        //        );

        //        if (!isValid)
        //        {
        //            context.AddFailure("TeamBResult", "Some players listed for Team B are not valid members of the team in this tournament.");
        //        }
        //    });

        //// Step 10: Validate All Referees are from the correct tournament and match
        //RuleFor(x => x)
        //    .CustomAsync(async (command, context, cancellation) =>
        //    {
        //        if (!TryGetMatch(context, out MatchSchedule match))
        //            return;

        //        var refereeIds = command.Dto.TeamARecord.CardsViolations
        //            .Select(x => x.TournamentRefreeId)
        //            .Concat(command.Dto.TeamBRecord.CardsViolations.Select(x => x.TournamentRefreeId))
        //            .Where(id => id != 0)
        //            .Distinct()
        //            .ToList();

        //        if (!refereeIds.Any())
        //        {
        //            context.AddFailure("TournamentRefereeIds", "No referees were provided in card violations.");
        //            return;
        //        }

        //        var isValid = await _matchRepo.ValidateTournamentRefereesAsync(
        //            match.MatchScheduleId,
        //            refereeIds
        //        );

        //        if (!isValid)
        //        {
        //            context.AddFailure("TournamentRefereeIds", "One or more referees are not assigned to the match.");
        //        }
        //    });
    }

}

public class TeamMatchResultDTOValidator : AbstractValidator<TeamMatchRecordDTO>
{
    public TeamMatchResultDTOValidator()
    {
        RuleFor(x => x.TournamentTeamId)
            .GreaterThan(0)
            .WithMessage("TournamentTeamId must be greater than 0");

        RuleFor(x => x.GoalsFor)
            .GreaterThanOrEqualTo(0)
            .WithMessage("GoalsFor must be greater than or equal to 0");

        RuleFor(x => x.GoalAgainst)
            .GreaterThanOrEqualTo(0)
            .WithMessage("GoalAgainst must be greater than or equal to 0");

        RuleFor(x => x.AcquisitionRate)
            .InclusiveBetween(0, 100)
            .WithMessage("AcquisitionRate must be between 0 and 100");

        RuleFor(x => x.BestPlayer)
            .GreaterThan(0)
            .When(x => x.BestPlayer.HasValue)
            .WithMessage("BestPlayer ID must be greater than 0");

        // Additional validations as needed
        RuleFor(x => x.ShotsOnGoal)
            .NotNull()
            .WithMessage("ShotsOnGoal cannot be null");
    }
}

public class ShotOnGoalDTOValidator : AbstractValidator<ShotOnGoalDTO>
{
    public ShotOnGoalDTOValidator()
    {
        RuleFor(x => x.Time)
            .NotNull()
            .WithMessage("Shot time is required");

        RuleFor(x => x.PlayerTeamId)
            .GreaterThan(0)
            .WithMessage("PlayerTeamId must be greater than 0");

        RuleFor(x => x.GoalkeeperTeamId)
            .GreaterThan(0)
            .WithMessage("GoalkeeperTeamId must be greater than 0");

        // Additional validation for whether the shot is a goal
        RuleFor(x => x.IsGoal)
            .NotNull()
            .WithMessage("IsGoal property must be specified");
    }
}