using TheDesignator.Domain.Repositories;

namespace TheDesignator.Infrastructure.DataAccess;

internal sealed class UnityOfWork : IUnityOfWork
{
    private readonly TheDesignatorContext _theDesignatorContext;

    public UnityOfWork(TheDesignatorContext theDesignatorContext)
    {
        _theDesignatorContext = theDesignatorContext;
    }

    public async Task Commit()
    {
        await _theDesignatorContext.SaveChangesAsync();
    }
}
