using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stefanini.DataStorage.Repositories
{
    public interface IRepository<T> where T : class
    {
        void EntityState(T model, EntityState state);

        void Add(T model);

        void Remove(T model);

        T Get(int id);

        Task<T> GetAsync(int id);

        T GetSingle(Expression<Func<T, bool>> whereCondition, string include = "");

        Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition, string include = "");

        List<T> GetAll();

        Task<List<T>> GetAllAsync();

        List<T> GetAll(Expression<Func<T, bool>> whereCondition);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition);

        void SaveChanges();

        void SaveChangesAsync();

    }

}