using FluentValidation;

namespace Ces_Platform_Server_Side.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.Name)
        .NotEmpty().WithMessage("Name is Required")
        .Length(2,63).WithMessage("Username must be between 2 and 63 characters.");

        RuleFor(u => u.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(u => u.Password)
        .NotEmpty().WithMessage("Password is required.")
        .Length(8,128).WithMessage("Password must be between 8 and 128 characters.");

        RuleFor(u => u.Role)
        .IsInEnum().WithMessage("invalid role value");
    }
}
