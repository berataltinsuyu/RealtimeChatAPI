using FluentValidation;
using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.API.Validators;

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(request => request.Content)
            .NotEmpty()
            .WithMessage("Mesaj içeriği boş olamaz.")
            .MaximumLength(1000)
            .WithMessage("Mesaj içeriği en fazla 1000 karakter olabilir.");
    }
}