using Moq;
using TheDesignator.Domain.Security.PasswordHashing;

namespace CommonTestUtilities.Security;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _mock;

    public IPasswordHasherBuilder()
    {
        _mock = new Mock<IPasswordHasher>();
        _mock.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("hasherPassword");
    }

    public void VerifyPassword(string password)
    {
        _mock.Setup(p => p.VerifyPassword(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordHasher Build() => _mock.Object;
}
