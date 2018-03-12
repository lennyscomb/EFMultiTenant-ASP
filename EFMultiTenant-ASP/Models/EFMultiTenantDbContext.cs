using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace EFMultiTenant.Models
{
    public class EFMultiTenantDbContext : DbContext, IUnitOfWork 
    {
        public EFMultiTenantDbContext() : base("EFMultiTenantDbContext")
        {

        }

        public DbSet<Customer> Customers { get; set; }

        public void AddEntity(IEntity entity)
        {
            Set(entity.GetType()).Add(entity);
        }

        public T FindEntity<T>(int id) where T : class
        {
            return Set<T>().Find(id);
        }

        public void DeleteEntity(IEntity entity)
        {
            Set(entity.GetType()).Remove(entity);
        }

        public IQueryable<T> Queryable<T>() where T : class
        {
            return Set<T>();
        }

        public List<T> FindAll<T>() where T : class
        {
            return Set<T>().ToList();
        }

        public List<T> CacheQueryable<T>(Func<T, bool> predicate) where T : class
        {
            return ((IObjectContextAdapter)this).ObjectContext.
                ObjectStateManager.GetObjectStateEntries(
                    EntityState.Added |
                    EntityState.Modified |
                    EntityState.Unchanged).
                Where(ent => ent.Entity is T).
                Select(ent => ent.Entity as T).
                Where(predicate).ToList();
        }

        public List<T> RunSQLQuery<T>(string query)
        {
            return Database.SqlQuery<T>(query).ToList();
        }

        public Database GetDatabase()
        {
            return Database;
        }

        public static EFMultiTenantDbContext Create()
        {
            return new EFMultiTenantDbContext();
        }
    }
}
