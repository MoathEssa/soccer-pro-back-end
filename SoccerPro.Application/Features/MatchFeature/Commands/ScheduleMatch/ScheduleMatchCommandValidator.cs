using FluentValidation;
using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.Features.MatchFeature.Commands.ScheduleMatch
{
    public class ScheduleMatchCommandValidator : AbstractValidator<ScheduleMatchCommand>
    {
        public ScheduleMatchCommandValidator()
        {
            RuleFor(x => x.TournamentId)
                .GreaterThan(0).WithMessage("TournamentId must be greater than 0.");

            RuleFor(x => x.TournamentPhase)
                .Must(value => Enum.IsDefined(typeof(TournamentPhase), value))
                .WithMessage("Invalid TournamentPhase value.");

            RuleFor(x => x.TournamentTeamIdA)
                .GreaterThan(0).WithMessage("TournamentTeamIdA must be greater than 0.");

            RuleFor(x => x.TournamentTeamIdB)
                .GreaterThan(0).WithMessage("TournamentTeamIdB must be greater than 0.")
                .NotEqual(x => x.TournamentTeamIdA)
                .WithMessage("TournamentTeamIdB must be different from TournamentTeamIdA.");

            RuleFor(x => x.Date)
                .Must(date => date.Date >= DateTime.Today)
                .WithMessage("Match date cannot be in the past.");

            RuleFor(x => x.FieldId)
                .GreaterThan(0).WithMessage("FieldId must be greater than 0.");
        }
    }
}
