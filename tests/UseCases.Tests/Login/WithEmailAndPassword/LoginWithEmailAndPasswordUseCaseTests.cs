using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using Shouldly;
using System.Net;
using TheDesignator.Application.UseCases.Login.WithEmailAndPassword;
using TheDesignator.Exception;
using TheDesignator.Exception.ExceptionsBase;

namespace UseCases.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestLoginJsonBuilder.Build();
        var (user, _) = UserBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(request.Password, user);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }

    [Fact]
    public async Task ShouldThrowException_WhenUserDontExist()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);
        exception.GetErrorMessages().ShouldSatisfyAllConditions(e =>
        {
            e.Count.ShouldBe(1);
            e.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    [Fact]
    public async Task ShouldThrowException_WhenPasswordIsIncorrect()
    {
        var request = RequestLoginJsonBuilder.Build();
        var (user, _) = UserBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user: user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.Unauthorized);
        exception.GetErrorMessages().ShouldSatisfyAllConditions(e =>
        {
            e.Count.ShouldBe(1);
            e.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }

    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null, TheDesignator.Domain.Entities.User? user = null)
    {
        var passwordHasherBuilder = new IPasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if(user is not null)
            userReadOnlyRepositoryBuilder.GetByEmail(user);

        if(!string.IsNullOrEmpty(password))
            passwordHasherBuilder.VerifyPassword(password);

        return new LoginWithEmailAndPasswordUseCase(passwordHasherBuilder.Build(), userReadOnlyRepositoryBuilder.Build());
    }
}
