using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EFMultiTenant.Models
{
    public interface IUnitOfWork : IDisposable
    {
        Database GetDatabase();
        void AddEntity(IEntity entity);
        T FindEntity<T>(int id) where T : class;
        void DeleteEntity(IEntity entity);
        IQueryable<T> Queryable<T>() where T : class;
        List<T> FindAll<T>() where T : class;
        int SaveChanges();
        List<T> CacheQueryable<T>(Func<T, bool> predicate) where T : class;
        List<T> RunSQLQuery<T>(string query);

    }
}
