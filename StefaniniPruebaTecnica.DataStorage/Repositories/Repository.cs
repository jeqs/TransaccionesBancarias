using Stefanini.DataStorage.Contexts;
using Stefanini.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stefanini.DataStorage.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;

        public Repository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        private StefaniniDbContext<TEntity> DbContext
        {
            get
            {
                var dbContext = _ambientDbContextLocator.Get<StefaniniDbContext<TEntity>>();

                if (dbContext == null)
                    throw new InvalidOperationException("Sin contexto de tipo UserManagement DbContext. Esto significa que este método ha sido llamado fuera del alcance de una DbContextScope. Un repositorio solamente se debe acceder dentro del alcance de un DbContextScope, que se ocupa de la creación de las instancias DbContext que los repositorios necesitan y ponerlos a disposición como contextos. Esto es lo que garantiza que, para cualquier tipo DbContext derivado dado, la misma instancia se utiliza en toda la duración de una transacción. Para solucionar este problema, utilice IDbContextFactory en su método de servicio de la lógica de negocio de alto nivel para crear un DbContextScope que envuelve toda la transacción que implementa el método de servicio. A continuación, acceder a este repositorio dentro de ese contexto. Consulte los comentarios del archivo IDbContextScope.cs para más detalles.");

                return dbContext;
            }
        }

        public void EntityState(TEntity model, EntityState state)
        {
            DbContext.Entry(model).State = state;
        }

        public void Add(TEntity model)
        {
            DbContext.Model.Add(model);
        }

        public void Remove(TEntity model)
        {
            DbContext.Model.Remove(model);
        }

        public TEntity Get(int id)
        {
            return DbContext.Model.Find(id);
        }

        public Task<TEntity> GetAsync(int id)
        {
            return DbContext.Model.FindAsync(id);
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> whereCondition, string include = "")
        {
            if (include == "")
                return DbContext.Model.Where(whereCondition).FirstOrDefault();
            else
                return DbContext.Model.Include(include).Where(whereCondition).FirstOrDefault();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> whereCondition, string include = "")
        {
            if (include == "")
                return await DbContext.Model.Where(whereCondition).FirstOrDefaultAsync();
            else
                return await DbContext.Model.Include(include).Where(whereCondition).FirstOrDefaultAsync();
        }

        public List<TEntity> GetAll()
        {
            return DbContext.Model.ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbContext.Model.ToListAsync();
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> whereCondition)
        {
            return DbContext.Model.Where(whereCondition).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> whereCondition)
        {
            return await DbContext.Model.Where(whereCondition).ToListAsync();
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public async void SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

    }

}
