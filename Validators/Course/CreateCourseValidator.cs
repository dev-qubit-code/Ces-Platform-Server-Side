using Ces_Platform_Server_Side.Requests.Course;
using FluentValidation;
using Microsoft.Identity.Client.Extensibility;

namespace Ces_Platform_Server_Side.Validators.Course
{
    public class CreateCourseValidator :AbstractValidator<CreateCourseRequest>
    {
        public CreateCourseValidator()
        {
            RuleFor(C => C.Name)
            .NotEmpty().WithMessage("Name is Requaird")
            .Length(2, 65).WithMessage("Name Must be between 2 and 65");

        }
    }
}
