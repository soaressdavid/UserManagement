using FluentValidation;
using UserManagement.API.DTOs.User;

namespace UserManagement.API.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name é obrigatório.")
                .MaximumLength(100).WithMessage("Nome pode ter no máximo 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha precisa ter no mínimo 6 caracteres.");
        }
    }
}
