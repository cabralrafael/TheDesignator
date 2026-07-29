using FluentValidation.Results;
using Mapster;
using System.Formats.Asn1;
using TheDesignator.Communication.Requests;
using TheDesignator.Communication.Responses;
using TheDesignator.Domain.Repositories;
using TheDesignator.Domain.Repositories.User;
using TheDesignator.Domain.Security.PasswordHashing;
using TheDesignator.Exception;
using TheDesignator.Exception.ExceptionsBase;

namespace TheDesignator.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnityOfWork _unityOfWork;

    public RegisterUserAccountUseCase(IPasswordHasher passwordHasher, 
                                      IUserWriteOnlyRepository userWriteOnlyRepository, 
                                      IUnityOfWork unityOfWork, 
                                      IUserReadOnlyRepository userReadOnlyRepository)
    {
        _passwordHasher = passwordHasher;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unityOfWork = unityOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordHasher.HashPassword(request.Password);

        await _userWriteOnlyRepository.Add(user);

        await _unityOfWork.Commit();

        var result = new ResponseRegisteredUserJson
        {
            Name = user.Name
        };

        return result;
    }

    private async Task ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountUseCaseValidator();

        var result = validator.Validate(request);

        var emailExists = await _userReadOnlyRepository.ExistsActiveUserEmail(request.Email);
        if(emailExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));
        }

        if(result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
