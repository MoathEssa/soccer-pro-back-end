using FluentValidation;

namespace SoccerPro.Application.Features.ManagersFeature.Commands.AddManager;

public class AddManagerCommandValidator : AbstractValidator<AddManagerCommand>
{
    public AddManagerCommandValidator()
    {
        RuleFor(x => x.AddManagerDTO.KFUPMId)
            .NotEmpty().WithMessage("KFUPM ID is required")
            .Length(9).WithMessage("KFUPM ID must be exactly 9 characters");

        RuleFor(x => x.AddManagerDTO.UserName)
.NotEmpty().WithMessage("Username is required.")
.EmailAddress().WithMessage("Username must be a valid email address.");

        RuleFor(x => x.AddManagerDTO.IntialPassword)
            .NotEmpty().WithMessage("IntialPassword is required.")
            .MinimumLength(6).WithMessage("IntialPassword must be at least 6 characters long.");


        RuleFor(x => x.AddManagerDTO.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

        RuleFor(x => x.AddManagerDTO.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters");

        RuleFor(x => x.AddManagerDTO.SecondName)
            .MaximumLength(50).WithMessage("Second name must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.AddManagerDTO.SecondName));

        RuleFor(x => x.AddManagerDTO.ThirdName)
            .MaximumLength(50).WithMessage("Third name must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.AddManagerDTO.ThirdName));

        RuleFor(x => x.AddManagerDTO.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past");

        RuleFor(x => x.AddManagerDTO.PersonalContactInfos)
            .NotEmpty().WithMessage("At least one contact information is required")
            .Must(x => x.Count <= 5).WithMessage("Maximum 5 contact information entries allowed");

    }
}