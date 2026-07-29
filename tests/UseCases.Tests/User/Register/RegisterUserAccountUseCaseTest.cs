using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using Shouldly;
using TheDesignator.Application.UseCases.User.Register;
using TheDesignator.Domain.Repositories.User;
using TheDesignator.Exception;
using TheDesignator.Exception.ExceptionsBase;

namespace UseCases.Tests.User.Register;

public class RegisterUserAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Tokens.AccessToken.ShouldBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();

    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS);
        });
    }

    private RegisterUserAccountUseCase CreateUseCase(string? emailThatAlreadyExists = null)
    {
        var passwordHasher = new IPasswordHasherBuilder().Build();
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();
        var unityOfWork = IUnityOfWorkBuilder.Build();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();

        if(!string.IsNullOrEmpty(emailThatAlreadyExists))
            userReadOnlyRepositoryBuilder.ExistsActiveUserEmail(emailThatAlreadyExists);

        return new RegisterUserAccountUseCase(passwordHasher, userWriteOnlyRepository, unityOfWork, userReadOnlyRepositoryBuilder.Build());
    }
}
