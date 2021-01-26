using System.Data.Entity;

namespace Stefanini.Infrastructure.Interfaces
{

    public interface IDbContextFactory
    {
        TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext;
    }
}
