using Ces_Platform_Server_Side.Requests;
using FluentValidation;

namespace Ces_Platform_Server_Side.Validators
{
    public class UpdateNoteRequestValidatior : AbstractValidator<CreateNoteRequest>
    {
        public UpdateNoteRequestValidatior()
        {
            RuleFor(u => u.NoteName)
            .NotEmpty().WithMessage("Name is Required")
            .Length(2, 63).WithMessage("Teacher name must be between 2 and 63 characters.");

            RuleFor(n => n.TeacherId).NotEmpty().WithMessage("Teacher ID Is Requaird");

            RuleFor(n => n.CourseId).NotEmpty().WithMessage("Course ID Is Requaird");
        }
    }
}
