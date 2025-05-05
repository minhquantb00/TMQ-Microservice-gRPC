using FluentValidation;
using TMQ.AccountCommands.Commands;

namespace TMQ.AccountManager.Validations
{
    public class ResetPasswordRequestValidator : AbstractValidator<SetPasswordCommand>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.ObjectId).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().Length(8, 50)
                .Matches("[A-Z]").WithMessage("password rule uppercase invalid")
                .Matches("[a-z]").WithMessage("password rule lowercase invalid")
                .Matches("[0-9]").WithMessage("password rule digit invalid")
                .Matches("[!@#$%^&*().,]")
                .WithMessage("password rule special invalid");
        }

        public static FluentValidation.Results.ValidationResult ValidateModel(SetPasswordCommand request)
        {
            var validationResult = new ResetPasswordRequestValidator().Validate(request);
            return validationResult;
        }
    }
}
