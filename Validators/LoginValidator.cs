using FluentValidation;
using UserManagement.API.DTOs.Auth;

namespace UserManagement.API.Validators
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.");
        }
    }
}
