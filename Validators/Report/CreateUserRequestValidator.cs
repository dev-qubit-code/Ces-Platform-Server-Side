using Ces_Platform_Server_Side.Requests;
using FluentValidation;

namespace Ces_Platform_Server_Side.Validators;

public class CreateReportRequestValidator : AbstractValidator<CreateReportRequest>
{
    public CreateReportRequestValidator()
    {
        RuleFor(u => u.Title)
        .NotEmpty().WithMessage("Title is Required")
        .Length(2,50).WithMessage("Title must be between 2 and 50 characters.");
     
        RuleFor(u => u.Description)
        .NotEmpty().WithMessage("Description is Required")
        .Length(2,255).WithMessage("Description must be between 2 and 255 characters.");

        RuleFor(u => u.Priority)
        .IsInEnum().WithMessage("invalid priority value");
    }
}
