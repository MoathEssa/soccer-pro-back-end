namespace SoccerPro.Application.Features.PlayerFeature.Commands.AddPlayer;
using FluentValidation;

public class AddPlayerCommandValidator : AbstractValidator<AddPlayerCommand>
{
    public AddPlayerCommandValidator()
    {

        RuleFor(x => x.AddPlayerDTO.KFUPMId)
            .NotEmpty().WithMessage("KFUPM ID is required.")
            .Matches(@"^\d{9}$").WithMessage("KFUPM ID must be exactly 9 digits.");

        RuleFor(x => x.AddPlayerDTO.UserName)
.NotEmpty().WithMessage("Username is required.")
.EmailAddress().WithMessage("Username must be a valid email address.");

        RuleFor(x => x.AddPlayerDTO.IntialPassword)
            .NotEmpty().WithMessage("IntialPassword is required.")
            .MinimumLength(6).WithMessage("IntialPassword must be at least 6 characters long.");


        RuleFor(x => x.AddPlayerDTO.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.AddPlayerDTO.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.AddPlayerDTO.DateOfBirth)
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.AddPlayerDTO.NationalityId)
            .GreaterThan(0).When(x => x.AddPlayerDTO.NationalityId.HasValue)
            .WithMessage("Nationality ID must be greater than 0 if provided.");

        RuleFor(x => x.AddPlayerDTO.PlayerType)
            .IsInEnum().WithMessage("Invalid Player Type.");

        RuleFor(x => x.AddPlayerDTO.DepartmentId)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0.");

        RuleFor(x => x.AddPlayerDTO.PlayerStatus)
            .IsInEnum().WithMessage("Invalid Player Status.");

        RuleFor(x => x.AddPlayerDTO.PersonalContactInfos)
            .NotEmpty().WithMessage("At least one personal contact info is required.");
    }
}
