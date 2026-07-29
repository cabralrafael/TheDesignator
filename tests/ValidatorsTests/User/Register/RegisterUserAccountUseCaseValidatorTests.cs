using CommonTestUtilities.Requests;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using TheDesignator.Application.UseCases.User.Register;
using TheDesignator.Exception;

namespace ValidatorsTests.User.Register;

public class RegisterUserAccountUseCaseValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var validator = new RegisterUserAccountUseCaseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("              ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Intentional because is a unit test")]
    public void ValidateShouldBeError_WhenNameIsEmpty(string name)
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = name;

        var validator = new RegisterUserAccountUseCaseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(e =>
        {
            e.Count.ShouldBe(1);
            e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData("              ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Intentional because is a unit test")]
    public void ValidateShouldBeError_WhenEmailIsEmpty(string email)
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = email;

        var validator = new RegisterUserAccountUseCaseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(e =>
        {
            e.Count.ShouldBe(1);
            e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED));
        });
    }

    [Fact]
    public void ValidateShouldBeError_WhenPasswordIsEmpty()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Password = String.Empty;

        var validator = new RegisterUserAccountUseCaseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(e =>
        {
            e.Count.ShouldBe(1);
            e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED));
        });
    }

    [Fact]
    public void ValidateShouldBeError_WhenEmailIsInvalid()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = request.Email.Replace("@", "");

        var validator = new RegisterUserAccountUseCaseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(e =>
        {
            e.Count.ShouldBe(1);
            e.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_INVALID));
        });
    }
}
