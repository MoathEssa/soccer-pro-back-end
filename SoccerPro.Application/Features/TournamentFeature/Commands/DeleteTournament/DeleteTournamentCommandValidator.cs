using FluentValidation;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.DeleteTournament
{
    public class DeleteTournamentCommandValidator : AbstractValidator<DeleteTournamentCommand>
    {
        public DeleteTournamentCommandValidator()
        {
            RuleFor(x => x.TournamentId)
                .NotEmpty()
                .WithMessage("Tournament ID is required")
                .GreaterThan(0)
                .WithMessage("Tournament ID must be gratter than zero.");
        }
    }

}
