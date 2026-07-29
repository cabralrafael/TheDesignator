namespace TheDesignator.Domain.Repositories;

public interface IUnityOfWork
{
    Task Commit();
}
