using Stefanini.Infrastructure.Enums;
using Stefanini.Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace Stefanini.Infrastructure.Implementations
{
    public class DbContextScope : IDbContextScope
    {
        private bool _disposed;
        private bool _readOnly;
        private bool _completed;
        private bool _nested;
        private DbContextScope _parentScope;
        private DbContextCollection _dbContexts;

        public IDbContextCollection DbContexts { get { return _dbContexts; } }

        public DbContextScope(IDbContextFactory dbContextFactory = null) :
            this(joiningOption: DbContextScopeOption.JoinExisting, readOnly: false, isolationLevel: null, dbContextFactory: dbContextFactory)
        { }

        public DbContextScope(bool readOnly, IDbContextFactory dbContextFactory = null)
            : this(joiningOption: DbContextScopeOption.JoinExisting, readOnly: readOnly, isolationLevel: null, dbContextFactory: dbContextFactory)
        { }

        public DbContextScope(DbContextScopeOption joiningOption, bool readOnly, IsolationLevel? isolationLevel, IDbContextFactory dbContextFactory = null)
        {
            if (isolationLevel.HasValue && joiningOption == DbContextScopeOption.JoinExisting)
                throw new ArgumentException("No puede unirse a una DbContextScope cuando se requiere una operación de base de datos explícita. Al solicitar que las transacciones de bases de datos explícitos a utilizar (establece el parámetro 'isolationLevel'), si no quiere preguntar por el contexto a unirse (use 'joinAmbient' parámetro en false).");

            _disposed = false;
            _completed = false;
            _readOnly = readOnly;

            _parentScope = GetAmbientScope();
            if (_parentScope != null && joiningOption == DbContextScopeOption.JoinExisting)
            {
                if (_parentScope._readOnly && !this._readOnly)
                    throw new InvalidOperationException("No se puede anidar un DbContextScope en modo lectura/escritura dentro de un contexto DbContextScope de solo lectura.");

                _nested = true;
                _dbContexts = _parentScope._dbContexts;
            }
            else
            {
                _nested = false;
                _dbContexts = new DbContextCollection(readOnly, isolationLevel, dbContextFactory);
            }

            SetAmbientScope(this);
        }

        public int SaveChanges()
        {
            if (_disposed)
                throw new ObjectDisposedException("DbContextScope");

            if (_completed)
                throw new InvalidOperationException("You cannot call SaveChanges() more than once on a DbContextScope. A DbContextScope is meant to encapsulate a business transaction: create the scope at the start of the business transaction and then call SaveChanges() at the end. Calling SaveChanges() mid-way through a business transaction doesn't make sense and most likely mean that you should refactor your service method into two separate service method that each create their own DbContextScope and each implement a single business transaction.");

            var c = 0;
            if (!_nested)
                c = CommitInternal();

            _completed = true;

            return c;
        }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancelToken)
        {
            if (cancelToken == null)
                throw new ArgumentNullException("cancelToken");

            if (_disposed)
                throw new ObjectDisposedException("DbContextScope");

            if (_completed)
                throw new InvalidOperationException("You cannot call SaveChanges() more than once on a DbContextScope. A DbContextScope is meant to encapsulate a business transaction: create the scope at the start of the business transaction and then call SaveChanges() at the end. Calling SaveChanges() mid-way through a business transaction doesn't make sense and most likely mean that you should refactor your service method into two separate service method that each create their own DbContextScope and each implement a single business transaction.");

            var c = 0;
            if (!_nested)
                c = await CommitInternalAsync(cancelToken).ConfigureAwait(false);

            _completed = true;
            return c;
        }

        private int CommitInternal()
        {
            return _dbContexts.Commit();
        }

        private Task<int> CommitInternalAsync(CancellationToken cancelToken)
        {
            return _dbContexts.CommitAsync(cancelToken);
        }

        private void RollbackInternal()
        {
            _dbContexts.Rollback();
        }

        public void RefreshEntitiesInParentScope(IEnumerable entities)
        {
            if (entities == null)
                return;

            if (_parentScope == null)
                return;

            // El contexto matriz que utilice los mismos casos DbContext como nosotros - no hay necesidad de volver a cargar nada
            if (_nested) 
                return;
            foreach (IObjectContextAdapter contextInCurrentScope in _dbContexts.InitializedDbContexts.Values)
            {
                var correspondingParentContext =
                    _parentScope._dbContexts.InitializedDbContexts.Values.SingleOrDefault(parentContext => parentContext.GetType() == contextInCurrentScope.GetType())
                    as IObjectContextAdapter;

                // Sin DbContext de este tipo ha sido creado en el contexto padre todavía. Por lo tanto no hay necesidad de actualizar nada para este tipo DbContext.
                if (correspondingParentContext == null)
                    continue; 

                foreach (var toRefresh in entities)
                {
                    ObjectStateEntry stateInCurrentScope;
                    if (contextInCurrentScope.ObjectContext.ObjectStateManager.TryGetObjectStateEntry(toRefresh, out stateInCurrentScope))
                    {
                        var key = stateInCurrentScope.EntityKey;

                        // Ahora podemos ver si existe esa entidad en la instancia 
                        // primaria de DbContext y actualizarla.
                        ObjectStateEntry stateInParentScope;
                        if (correspondingParentContext.ObjectContext.ObjectStateManager.TryGetObjectStateEntry(key, out stateInParentScope))
                        {
                            if (stateInParentScope.State == System.Data.Entity.EntityState.Unchanged)
                                correspondingParentContext.ObjectContext.Refresh(RefreshMode.StoreWins, stateInParentScope.Entity);
                        }
                    }
                }
            }
        }

        public async Task RefreshEntitiesInParentScopeAsync(IEnumerable entities)
        {
             
            if (entities == null)
                return;

            if (_parentScope == null)
                return;

            if (_nested)
                return;

            foreach (IObjectContextAdapter contextInCurrentScope in _dbContexts.InitializedDbContexts.Values)
            {
                var correspondingParentContext =
                    _parentScope._dbContexts.InitializedDbContexts.Values.SingleOrDefault(parentContext => parentContext.GetType() == contextInCurrentScope.GetType())
                    as IObjectContextAdapter;

                if (correspondingParentContext == null)
                    continue;

                foreach (var toRefresh in entities)
                {
                    ObjectStateEntry stateInCurrentScope;
                    if (contextInCurrentScope.ObjectContext.ObjectStateManager.TryGetObjectStateEntry(toRefresh, out stateInCurrentScope))
                    {
                        var key = stateInCurrentScope.EntityKey;

                        ObjectStateEntry stateInParentScope;
                        if (correspondingParentContext.ObjectContext.ObjectStateManager.TryGetObjectStateEntry(key, out stateInParentScope))
                        {
                            if (stateInParentScope.State == System.Data.Entity.EntityState.Unchanged)
                            {
                                await correspondingParentContext.ObjectContext.RefreshAsync(RefreshMode.StoreWins, stateInParentScope.Entity).ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            // Commit / Rollback y dispose de todas las instancias de nuestro DbContext
            if (!_nested)
            {
                if (!_completed)
                {
                    // Hacer todo lo posible para limpiar todo lo que puede, pero no lanzar aquí, ya que es demasiado tarde de todos modos.
                    try
                    {
                        if (_readOnly)
                            CommitInternal();
                        else
                            RollbackInternal();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }

                    _completed = true;
                }

                _dbContexts.Dispose();
            }

            var currentAmbientScope = GetAmbientScope();
            if (currentAmbientScope != this) // Este es un error de programación grave. 
                throw new InvalidOperationException("DbContextScope instancias, deben eliminarse en el orden en el que fueron creados!");

            RemoveAmbientScope();

            if (_parentScope != null)
            {
                if (_parentScope._disposed)
                {
                    var message = @"ERROR - Al intentar disponer un DbContextScope, encontramos que nuestro padre DbContextScope ya se ha dispuesto! Esto significa que alguien comenzó un flujo paralelo de ejecución (por ejemplo, crea una tarea TPL, creado un hilo o en cola un elemento de trabajo en el ThreadPool) en el contexto de un DbContextScope sin suprimir el contexto primero.
Con el fin de solucionar este problema:
1) Mira el seguimiento de la pila de abajo - este es el seguimiento de la pila de la tarea paralela en cuestión.
2) Para saber donde se creó esta tarea en paralelo.
3) Cambiar el código de modo que el contexto se suprime antes de crear la tarea paralela. Usted puede hacer esto con IDbContextScopeFactory.SuppressAmbientContext() (situar el bloque de código de creación de tarea paralela en este).

Stack Trace:
" + Environment.StackTrace;

                    System.Diagnostics.Debug.WriteLine(message);
                }
                else
                {
                    SetAmbientScope(_parentScope);
                }
            }

            _disposed = true;

        }

        #region Ambient Context Logic

        private static readonly string AmbientDbContextScopeKey = "AmbientDbcontext_" + Guid.NewGuid();

        private static readonly ConditionalWeakTable<InstanceIdentifier, DbContextScope> DbContextScopeInstances = new ConditionalWeakTable<InstanceIdentifier, DbContextScope>();

        private InstanceIdentifier _instanceIdentifier = new InstanceIdentifier();
        
        internal static void SetAmbientScope(DbContextScope newAmbientScope)
        {
            if (newAmbientScope == null)
                throw new ArgumentNullException("newAmbientScope");

            var current = CallContext.LogicalGetData(AmbientDbContextScopeKey) as InstanceIdentifier;

            if (current == newAmbientScope._instanceIdentifier)
                return;

            // Almacenar el nuevo identificador de la instancia en el contexto CallContext, por lo que es el ámbito
            CallContext.LogicalSetData(AmbientDbContextScopeKey, newAmbientScope._instanceIdentifier);

            // Realizar un seguimiento de este caso (o no hacer nada, si ya estamos rastreando)
            DbContextScopeInstances.GetValue(newAmbientScope._instanceIdentifier, key => newAmbientScope);
        }
        
        internal static void RemoveAmbientScope()
        {
            var current = CallContext.LogicalGetData(AmbientDbContextScopeKey) as InstanceIdentifier;
            CallContext.LogicalSetData(AmbientDbContextScopeKey, null);

            // Si había un contexto, podemos detener el seguimiento de ahora
            if (current != null)
            {
                DbContextScopeInstances.Remove(current);
            }
        }
        
        internal static void HideAmbientScope()
        {
            CallContext.LogicalSetData(AmbientDbContextScopeKey, null);
        }
        
        internal static DbContextScope GetAmbientScope()
        {
            //  Recuperar el identificador del contexto (si la hay)
            var instanceIdentifier = CallContext.LogicalGetData(AmbientDbContextScopeKey) as InstanceIdentifier;
            if (instanceIdentifier == null)
                return null; 

            DbContextScope ambientScope;
            if (DbContextScopeInstances.TryGetValue(instanceIdentifier, out ambientScope))
                return ambientScope;

            System.Diagnostics.Debug.WriteLine("Error de programación detectado. Se encontró una referencia a un DbContextScope ambiente en el CallContext pero no tenía una instancia para él en nuestra tabla DbContextScopeInstances. Lo más probable es que esta instancia de DbContextScope no se haya eliminado correctamente. La instancia de DbContextScope siempre se debe eliminar. Revise el código de cualquier instancia de DbContextScope que se use fuera de un bloque 'using' y corríjalo para que se eliminen todas las instancias de DbContextScope.");
            return null;
        }

        #endregion
    }

    internal class InstanceIdentifier : MarshalByRefObject
    { }
}

