using System;
using System.Data.Entity;

namespace Stefanini.Infrastructure.Interfaces
{
    /// <summary>
    /// Mantiene una lista cargue perezoso de las instancias DbContext.
    /// </summary>
    public interface IDbContextCollection : IDisposable
    {
		TDbContext Get<TDbContext>() where TDbContext : DbContext;
    }
}