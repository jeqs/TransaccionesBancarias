using log4net;
using Stefanini.DataStorage.Repositories;
using Stefanini.Infrastructure.Implementations;
using Stefanini.Infrastructure.Interfaces;
using Stefanini.Transverse;
using System;

namespace Stefanini.DataStorage.DataAccess
{
    public class BaseAccess<T> where T : class
    {
        internal static readonly ILog log = StefaniniExcepcion.GetLogger(typeof(T));
        internal readonly IDbContextScopeFactory dbContextScopeFactory;
        internal readonly IRepository<T> repository;

        public BaseAccess()
        {
            var _dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();
            var _repository = new Repository<T>(ambientDbContextLocator);

            if (_dbContextScopeFactory == null) throw new ArgumentNullException("dbContextScopeFactory");
            if (_repository == null) throw new ArgumentNullException("repository");

            dbContextScopeFactory = _dbContextScopeFactory;
            repository = _repository;
        }
    }
}
