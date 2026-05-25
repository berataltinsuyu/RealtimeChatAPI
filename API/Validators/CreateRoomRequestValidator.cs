using FluentValidation;
using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.API.Validators;

public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Oda adı zorunludur.")
            .MaximumLength(100)
            .WithMessage("Oda adı en fazla 100 karakter olabilir.");
    }
}