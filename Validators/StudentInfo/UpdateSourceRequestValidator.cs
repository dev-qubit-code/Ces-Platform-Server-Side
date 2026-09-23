using Ces_Platform_Server_Side.Requests;
using FluentValidation;

namespace Ces_Platform_Server_Side.Validators;

public class UpdateSourceRequestValidator : AbstractValidator<UpdateSourceRequest>
{
    public UpdateSourceRequestValidator()
    {
        RuleFor(si => si.Name)
        .NotEmpty().WithMessage("Name is Required")
        .Length(2,50).WithMessage("Source name must be between 2 and 50 characters.");
        
        RuleFor(si => si.Url)
        .NotEmpty().WithMessage("Url is Required")
        .Length(2,50).WithMessage("Source name must be between 2 and 50 characters.");

    }
}
