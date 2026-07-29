using Moq;
using TheDesignator.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class IUnityOfWorkBuilder
{
    public static IUnityOfWork Build()
    {
        var mock = new Mock<IUnityOfWork>();

        return mock.Object;
    }
}
