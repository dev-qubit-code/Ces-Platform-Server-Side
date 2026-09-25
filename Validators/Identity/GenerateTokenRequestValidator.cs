using FluentValidation;
using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Validators;

public class GenerateTokenRequestValidator : AbstractValidator<GenerateTokenRequest>
{
    public GenerateTokenRequestValidator()
    {

        RuleFor(u => u.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(u => u.Password)
        .NotEmpty().WithMessage("Password is required.")
        .Length(8,128).WithMessage("Password must be between 8 and 128 characters.");
        
    }
}
