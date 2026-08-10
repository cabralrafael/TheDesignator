namespace TheDesignator.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsActiveUserEmail(string email);

    Task<Entities.User?> GetByEmail(string email);
}