using FluentValidation;

namespace SoccerPro.Application.Features.TournamentFeature.Queries.FetchTournamentById
{

    public class FetchTournamentByIdQueryValidator : AbstractValidator<FetchTournamentByIdQuery>
    {
        public FetchTournamentByIdQueryValidator()
        {
            RuleFor(x => x.TournamentId)
                .GreaterThan(0)
                .WithMessage("TournamentId must be greater than zero.");
        }
    }

}
