using Moq;
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

    public IUserReadOnlyRepository Build() => _mock.Object;
}
