using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace StefaniniPruebaTecnica.DataStorage.BD
{
    public partial class StefaniniDBEntities : DbContext
    {
        public StefaniniDBEntities(bool lazyLoadingEnabled)
        {
            this.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;
        }

        public ObjectContext ObjectContext()
        {
            return (this as IObjectContextAdapter).ObjectContext;
        }
    }
}
