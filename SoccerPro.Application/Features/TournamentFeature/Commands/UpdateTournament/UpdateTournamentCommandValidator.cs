using FluentValidation;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.UpdateTournament
{
    public class UpdateTournamentCommandValidator : AbstractValidator<UpdateTournamentCommand>
    {
        public UpdateTournamentCommandValidator()
        {
            RuleFor(x => x.UpdateTournamentDTO.TournamentId)
                .GreaterThan(0)
                .WithMessage("TournamentId must be greater than zero.");

            RuleFor(x => x.UpdateTournamentDTO.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.UpdateTournamentDTO.StartDate)
                .NotEmpty()
                .WithMessage("StartDate is required.");

            RuleFor(x => x.UpdateTournamentDTO.EndDate)
                .NotEmpty()
                .WithMessage("EndDate is required.")
                .GreaterThanOrEqualTo(x => x.UpdateTournamentDTO.StartDate)
                .WithMessage("EndDate must be greater than or equal to StartDate.");
        }
    }
}