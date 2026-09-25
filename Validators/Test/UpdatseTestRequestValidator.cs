using Ces_Platform_Server_Side.Requests;
using FluentValidation;

namespace Ces_Platform_Server_Side.Validators 
{
    public class UpdatseTestRequestValidator : AbstractValidator<UpdateTestRequest>
    {
        public UpdatseTestRequestValidator()
        {

            RuleFor(r => r.TeacherId).NotNull();
            RuleFor(r => r.CourseId).NotNull();
        
            RuleFor(t => t.TestDate).NotNull().WithMessage("test date is required");
            RuleFor(t => t.Kind).NotNull().WithMessage("test date is required");
        }
    }
}
