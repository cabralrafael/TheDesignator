using FluentValidation;
using TheDesignator.Communication.Requests;
using TheDesignator.Exception;

namespace TheDesignator.Application.UseCases.User.Register;

public class RegisterUserAccountUseCaseValidator : AbstractValidator<RequestRegisterUserAccountJson>
{
    public RegisterUserAccountUseCaseValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);
        RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED);
        When(user => string.IsNullOrWhiteSpace(user.Email) == false, () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
        });
    }
}
