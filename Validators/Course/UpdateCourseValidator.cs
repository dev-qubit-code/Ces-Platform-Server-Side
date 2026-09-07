using Ces_Platform_Server_Side.Requests;
using FluentValidation;

namespace Ces_Platform_Server_Side.Validators
{
    public class UpdateCourseValidator:AbstractValidator<UpdateCourseRequest>
    {
        public UpdateCourseValidator()
        {
            RuleFor(C => C.Name)
           .NotEmpty().WithMessage("Name is Requaird")
           .Length(2, 65).WithMessage("Name Must be between 2 and 65");

        }
    }
}
