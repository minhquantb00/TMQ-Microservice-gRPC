using FluentValidation;
using System.Text.RegularExpressions;
using TMQ.AccountCommands.Commands;
using TMQ.Common;

namespace TMQ.AccountManager.Validations
{
    public class AccountChangeRequestValidator : AbstractValidator<AccountChangeCommand>
    {
        public AccountChangeRequestValidator()
        {
            RuleFor(x => x.ObjectId).NotEmpty().Length(3, 50);
            RuleFor(x => x.FullName).NotEmpty().Length(3, 50);
            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .Must(email => Regex.IsMatch(email.AsEmpty(), CommonUtility.EmailPattern))
                    .EmailAddress().WithMessage("email address is not valid")
                    ;
            });
            When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber).NotEmpty()
                    .Must(phoneNumber => CommonUtility.IsMobile(ref phoneNumber))
                    .WithMessage("phone number is not valid");
            });
        }

        public static FluentValidation.Results.ValidationResult ValidateModel(AccountChangeCommand request)
        {
            var validationResult = new AccountChangeRequestValidator().Validate(request);
            return validationResult;
        }
    }
}
