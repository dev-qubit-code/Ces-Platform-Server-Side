using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Validators.Constants;
using FluentValidation;

namespace Ces_Platform_Server_Side.Validators;

public class CreateArticleRequestValidator : AbstractValidator<CreateArticleRequest>
{
    public CreateArticleRequestValidator()
    {
        RuleFor(a => a.Title)
        .NotEmpty().WithMessage("Name is Required")
        .Length(2,50).WithMessage("Title must be between 2 and 50 characters.");
        
        RuleFor(a => a.Description)
        .NotEmpty().WithMessage("Description is Required")
        .Length(2,15000).WithMessage($"Description must be between 2 and {15000} characters.");

        RuleFor(a => a.Date)
        .NotEmpty().WithMessage("Date is Required");
        
        // RuleFor(a => a.Thumbnail)
        // .NotEmpty().WithMessage("Thumbnail is Required")
        // .Must(file => file.Length <= 5 * 1024 * 1024)
        // .WithMessage("Each image must be less than 5 MB.")
        // .Must(ConditionsConstants.BeGenuineImage) 
        // .WithMessage("This file is not a real image.most be in JPEG/JPG/PNG/WEBP/GIF/BMP/TIFF");
    }

    
}
