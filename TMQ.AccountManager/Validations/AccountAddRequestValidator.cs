using FluentValidation;
using System.Text.RegularExpressions;
using TMQ.AccountCommands.Commands;
using TMQ.Common;

namespace TMQ.AccountManager.Validations
{
    public class AccountAddRequestValidator : AbstractValidator<AccountAddCommand>
    {
        public AccountAddRequestValidator()
        {
            RuleFor(x => x.AccountType).IsInEnum();
            RuleFor(x => x.FullName).NotEmpty().Length(3, 50);

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .Must(email => Regex.IsMatch(email.AsEmpty(), CommonUtility.EmailPattern))
                    .WithMessage("email address is not valid")
                    ;
            });
            When(x => !string.IsNullOrEmpty(x.PhoneNumber),
                () =>
                {
                    RuleFor(x => x.PhoneNumber).NotEmpty()
                        .Must(phoneNumber => CommonUtility.IsMobile(ref phoneNumber))
                        .WithMessage("phone number is not valid");
                });
            RuleFor(x => x.Password).NotEmpty().Length(8, 50)
                .Matches("[A-Z]").WithMessage("password rule uppercase invalid")
                .Matches("[a-z]").WithMessage("password rule lowercase invalid")
                .Matches("[0-9]").WithMessage("password rule digit invalid")
                .Matches("[!@#$%^&*().,]")
                .WithMessage("password rule special invalid")
                ;
        }

        public static FluentValidation.Results.ValidationResult ValidateModel(AccountAddCommand request)
        {
            var validationResult = new AccountAddRequestValidator().Validate(request);
            return validationResult;
        }
    }
}
