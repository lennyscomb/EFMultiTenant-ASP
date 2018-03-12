using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EntityFramework.DynamicFilters;

namespace EFMultiTenant.Models
{
    public class EFMultiTenantDbContext : DbContext, IUnitOfWork 
    {

        public static EFMultiTenantDbContext Create()
        {
            return new EFMultiTenantDbContext();
        }
        
        private Guid? _currentTenantId;

        public EFMultiTenantDbContext() : base("EFMultiTenantDbContext")
        {
            Init();
        }

        public EFMultiTenantDbContext(string connectionString) : base(connectionString)
        {
            Init();
        }

        public EFMultiTenantDbContext(string connectionString, DbCompiledModel model) : base(connectionString, model)
        {
            Init();
        }

        protected internal virtual void Init()
        {
            this.InitializeDynamicFilters();
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

        public override int SaveChanges()
        {
            var createdEntries = GetCreatedEntries();

            if (createdEntries.Any())
            {
                foreach (var createdEntry in createdEntries)
                {
                    var iSecuredByTenantEntry = createdEntry.Entity as ISecuredByTenant;
                    if (iSecuredByTenantEntry != null)
                        iSecuredByTenantEntry.SecuredByTenantId = _currentTenantId;
                }
            }

            return base.SaveChanges();
        }

        private IEnumerable<DbEntityEntry> GetCreatedEntries()
        {
            var createdEntries = ChangeTracker.Entries().Where(V =>
                EntityState.Added.HasFlag(V.State)
            );
            return createdEntries;
        }

        public void SetTenantId(Guid? tenantId)
        {
            _currentTenantId = tenantId;
            this.SetFilterScopedParameterValue("SecuredByTenant", "securedByTenantId", _currentTenantId);
            this.SetFilterGlobalParameterValue("SecuredByTenant", "securedByTenantId", _currentTenantId);
            //this.GetFilterParameterValue("SecuredByTenant", "securedByTenantId");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Filter("SecuredByTenant",
                (ISecuredByTenant securedByTenant, Guid? securedByTenantId) => securedByTenant.SecuredByTenantId == securedByTenantId,
                () => Guid.Empty);

            base.OnModelCreating(modelBuilder);
        }

    }
}
