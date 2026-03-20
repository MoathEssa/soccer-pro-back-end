using FluentValidation;

namespace SoccerPro.Application.Features.TournamentFeature.Commands.AddTournament;

public class AddTournamentCommandValidator : AbstractValidator<AddTournamentCommand>
{
    public AddTournamentCommandValidator()
    {

        RuleFor(x => x.AddTournamentDTO.Name)
            .NotEmpty().WithMessage("Tournament name is required.")
            .MaximumLength(100).WithMessage("Tournament name cannot exceed 100 characters.");

        RuleFor(x => x.AddTournamentDTO.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start date must be today or in the future.");

        RuleFor(x => x.AddTournamentDTO.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.AddTournamentDTO.StartDate).WithMessage("End date must be after start date.");
    }
}