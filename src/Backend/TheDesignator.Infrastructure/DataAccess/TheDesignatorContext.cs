using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TheDesignator.Domain.Entities;

[assembly: InternalsVisibleTo("WebApi.Tests")]
namespace TheDesignator.Infrastructure.DataAccess;

internal class TheDesignatorContext : DbContext
{
    public TheDesignatorContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

    public DbSet<User> Users { get; set; }
}
