using System;

namespace Stefanini.Infrastructure.Interfaces
{
    public interface IDbContextReadOnlyScope : IDisposable
    {
        IDbContextCollection DbContexts { get; }
    }
}