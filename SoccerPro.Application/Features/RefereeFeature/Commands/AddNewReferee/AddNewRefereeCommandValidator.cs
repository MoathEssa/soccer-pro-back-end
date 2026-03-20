using FluentValidation;

namespace SoccerPro.Application.Features.RefereeFeature.Commands.AddNewReferee;

public class AddNewRefereeCommandValidator : AbstractValidator<AddNewRefereeCommand>
{
    public AddNewRefereeCommandValidator()
    {
        RuleFor(x => x.RefereeDTO.KfupmId)
           .NotEmpty().WithMessage("KFUPM ID is required.")
           .Matches(@"^\d{9}$").WithMessage("KFUPM ID must be exactly 9 digits.");

        RuleFor(x => x.RefereeDTO.Username)
.NotEmpty().WithMessage("Username is required.")
.EmailAddress().WithMessage("Username must be a valid email address.");

        RuleFor(x => x.RefereeDTO.IntialPassword)
            .NotEmpty().WithMessage("IntialPassword is required.")
            .MinimumLength(6).WithMessage("IntialPassword must be at least 6 characters long.");


        RuleFor(x => x.RefereeDTO.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.RefereeDTO.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.RefereeDTO.DateOfBirth)
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.RefereeDTO.NationalityId)
            .GreaterThan(0).When(x => x.RefereeDTO.NationalityId.HasValue)
            .WithMessage("Nationality ID must be greater than 0 if provided.");


        RuleFor(x => x.RefereeDTO.PersonalContactInfos)
            .NotEmpty().WithMessage("At least one personal contact info is required.");
    }
}
