using Ces_Platform_Server_Side.Requests;
using FluentValidation;

namespace Ces_Platform_Server_Side.Validators;

public class UpdateStudentInfoRequestValidator : AbstractValidator<UpdateStudentInfoRequest>
{
    public UpdateStudentInfoRequestValidator()
    {
        RuleFor(si => si.Name)
        .NotEmpty().WithMessage("Name is Required")
        .Length(2,50).WithMessage("StudentInfo name must be between 2 and 50 characters.");

        RuleFor(si => si.About)
        .MaximumLength(255).WithMessage("About has a maximaum length of 255.");

        RuleFor(si => si.Major)
        .NotEmpty().WithMessage("Major is Required")
        .Length(2,50).WithMessage("Major must be between 2 and 50 characters.");

        RuleForEach(si => si.Skills)
        .NotEmpty().WithMessage("Skill is Required")
        .Length(2,50).WithMessage("Skill must be between 2 and 50 characters.");    

        RuleForEach(si => si.Sources)
        .SetValidator(new UpdateSourceRequestValidator());   
    }
}
