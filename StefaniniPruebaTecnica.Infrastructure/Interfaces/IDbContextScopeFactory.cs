using Stefanini.Infrastructure.Enums;
using System;
using System.Data;

namespace Stefanini.Infrastructure.Interfaces
{
    /// <summary>
    /// Los métodos de conveniencia para crear un nuevo DbContextScope. 
    /// Este es el método preferido para crear un DbContextScope.
    /// </summary>
    public interface IDbContextScopeFactory
    {
        IDbContextScope Create(DbContextScopeOption joiningOption = DbContextScopeOption.JoinExisting);

        IDbContextReadOnlyScope CreateReadOnly(DbContextScopeOption joiningOption = DbContextScopeOption.JoinExisting);

        IDbContextScope CreateWithTransaction(IsolationLevel isolationLevel);

        IDbContextReadOnlyScope CreateReadOnlyWithTransaction(IsolationLevel isolationLevel);

        IDisposable SuppressAmbientContext();
    }
}
