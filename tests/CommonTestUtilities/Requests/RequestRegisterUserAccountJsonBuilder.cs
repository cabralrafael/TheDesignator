using Bogus;
using TheDesignator.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserAccountJsonBuilder
{
    public static RequestRegisterUserAccountJson Build()
    {
        return new Faker<RequestRegisterUserAccountJson>()
            .RuleFor(r => r.Name, f => f.Person.FirstName)
            .RuleFor(r => r.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(r => r.Password, f => f.Internet.Password());
    }
}
