using FluentValidation;
using TMQ.AccountCommands.Commands;

namespace TMQ.AccountManager.Validations
{
    public class AccountChangePasswordCommandValidator : AbstractValidator<AccountChangePasswordCommand>
    {
        public AccountChangePasswordCommandValidator()
        {
            RuleFor(x => x.Password).NotEmpty().Length(8, 50)
                //.WithMessage("Mật khẩu phải lớn hơn 8 ký tự và nhỏ hơn 50 ký tự")
                //.Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhật một ký tự viết hoa")
                //.Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhật một ký tự viết thường")
                //.Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhật một ký tự là số")
                //.Matches("[!@#$%^&*().,]").WithMessage("Mật khẩu phải chứa ít nhật một ký tự là ký tự đặc biệt [!@#$%^&*().,]")
                ;
            RuleFor(x => x.NewPassword).NotEmpty().Length(8, 50);
            RuleFor(p => p.VerifyNewPassword).Equal(p => p.NewPassword);
        }

        public static FluentValidation.Results.ValidationResult ValidateModel(AccountChangePasswordCommand request)
        {
            var validationResult = new AccountChangePasswordCommandValidator().Validate(request);
            return validationResult;
        }
    }
}
