using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TheDesignator.Exception;
using TheDesignator.Infrastructure.DataAccess;

namespace WebApi.Tests.User.Register;

public class RegisterUserAccountTests : IClassFixture<TheDesignatorApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private const string REQUEST_URI = "/users";
    private readonly TheDesignatorContext _dbContext;

    public RegisterUserAccountTests(TheDesignatorApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<TheDesignatorContext>();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();

        var userExists = await _dbContext.Users.AnyAsync(u => u.Active && u.Name.Equals(request.Name) &&  u.Email.Equals(request.Email));
        userExists.ShouldBeTrue();
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(e =>
        {
            e.Count().ShouldBe(1);
            errors.ShouldContain(e => e.GetString() != null && e.GetString()!.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED));
        });

        var userExists = await _dbContext.Users.AnyAsync(u => u.Active && u.Name.Equals(request.Name) && u.Email.Equals(request.Email));
        userExists.ShouldBeFalse();
    }
}
