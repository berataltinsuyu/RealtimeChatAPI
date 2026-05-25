using FluentValidation;
using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.API.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty()
            .WithMessage("Kullanıcı adı zorunludur.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Şifre zorunludur.");
    }
}