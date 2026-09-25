using Ces_Platform_Server_Side.Requests;
using FluentValidation;

namespace Ces_Platform_Server_Side.Validators;

public class UpdateTestRequestValidator : AbstractValidator<UpdateTeacherRequest>
{
    public UpdateTestRequestValidator()
    {
        RuleFor(u => u.Name)
        .NotEmpty().WithMessage("Name is Required")
        .Length(2,63).WithMessage("Teacher name must be between 2 and 63 characters.");
    }
}
