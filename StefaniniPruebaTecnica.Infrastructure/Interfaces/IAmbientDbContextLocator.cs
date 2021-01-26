using System.Data.Entity;

namespace Stefanini.Infrastructure.Interfaces
{
    /// <summary>
    /// Los métodos de conveniencia para recuperar las instancias DbContext.
    /// </summary>
    public interface IAmbientDbContextLocator
    {
        TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}
