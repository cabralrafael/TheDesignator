using Moq;
using TheDesignator.Domain.Entities;
using TheDesignator.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class IUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock;

    public IUserReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistsActiveUserEmail(string email)
    {
        _mock.Setup(s => s.ExistsActiveUserEmail(email)).ReturnsAsync(true);
    }

    public void GetByEmail(User user)
    {
        _mock.Setup(r => r.GetByEmail(user.Email)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}
