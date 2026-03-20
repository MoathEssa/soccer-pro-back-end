using FluentValidation;
namespace SoccerPro.Application.Features.PlayerFeature.Queries.FetchPlayerById;
public class FetchPlayerByIdQueryValidator : AbstractValidator<FetchPlayerByIdQuery>
{
    public FetchPlayerByIdQueryValidator()
    {
        RuleFor(x => x.PlayerId)
            .NotEmpty()
            .WithMessage("Player ID is required.");
    }
}