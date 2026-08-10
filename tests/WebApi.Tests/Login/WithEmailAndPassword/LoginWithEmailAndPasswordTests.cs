using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TheDesignator.Communication.Requests;
using TheDesignator.Exception;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordTests : IClassFixture<TheDesignatorApplicationFactory>
{
    private const string REQUEST_URI = "/authentication";
    
    private readonly HttpClient _httpClient;
    private readonly UserIdentityManager _user1;

    public LoginWithEmailAndPasswordTests(TheDesignatorApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _user1.GetEmail(),
            Password = _user1.GetPassword()
        };

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_user1.GetName());
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();

    }

    [Fact]
    public async Task ShouldThrowException_WhenUserDontExist()
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(e => e.GetString() != null && e.GetString()!.Equals(ResourceMessagesException.VALIDATION_LOGIN_INVALID));
        });
    }
}
