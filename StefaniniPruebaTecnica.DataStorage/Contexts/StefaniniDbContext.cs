using System.Data.Entity;
using System.Reflection;

namespace Stefanini.DataStorage.Contexts
{
    public class StefaniniDbContext<TEntity> : DbContext where TEntity : class
    {
        // Un mapa de modelo de "TEntity"
        public DbSet<TEntity> Model { get; set; }

        public StefaniniDbContext() : base("name=StefaniniDBEntities")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetAssembly(typeof(StefaniniDbContext<TEntity>)));
        }
    }
}
