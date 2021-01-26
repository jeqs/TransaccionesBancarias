using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Stefanini.Infrastructure.Interfaces
{
    public interface IDbContextScope : IDisposable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancelToken);

        void RefreshEntitiesInParentScope(IEnumerable entities);

        Task RefreshEntitiesInParentScopeAsync(IEnumerable entities);

        IDbContextCollection DbContexts { get; }
    }
}
