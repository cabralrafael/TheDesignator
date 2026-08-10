using Bogus;
using CommonTestUtilities.Security;
using TheDesignator.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var (password, passwordHashed) = GenerateRandomPassword();

        var user = new Faker<User>()
           .RuleFor(u => u.Name, f => f.Person.FirstName)
           .RuleFor(u => u.Email, (f, user) => f.Internet.Email(user.Name))
           .RuleFor(u => u.Password, f => passwordHashed);

        return (user, password);
    }

    private static (string password, string passwordHashed) GenerateRandomPassword()
    {
        var passwordEncripter = new IPasswordHasherBuilder().Build();

        var password = new Faker().Internet.Password();

        return (password, passwordEncripter.HashPassword(password));
    }
}