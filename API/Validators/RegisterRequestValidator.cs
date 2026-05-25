using FluentValidation;
using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.API.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty()
            .WithMessage("Kullanıcı adı zorunludur.")
            .MinimumLength(3)
            .WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Şifre zorunludur.")
            .MinimumLength(6)
            .WithMessage("Şifre en az 6 karakter olmalıdır.");
    }
}