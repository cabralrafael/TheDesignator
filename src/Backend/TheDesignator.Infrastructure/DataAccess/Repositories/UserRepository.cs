using Microsoft.EntityFrameworkCore;
using TheDesignator.Domain.Entities;
using TheDesignator.Domain.Repositories.User;

namespace TheDesignator.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly TheDesignatorContext _theDesignatorContext;

    public UserRepository(TheDesignatorContext theDesignatorContext)
    {
        _theDesignatorContext = theDesignatorContext;
    }

    public async Task Add(User user)
    {
        await _theDesignatorContext.Users.AddAsync(user);
    }

    public async Task<bool> ExistsActiveUserEmail(string email) => await _theDesignatorContext.Users.AnyAsync(user => user.Active &&  user.Email == email);
}
