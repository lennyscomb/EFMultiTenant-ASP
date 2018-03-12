using System.Data.Entity;
using EFMultiTenant.Models;
using NUnit.Framework;

namespace EFMultiTenantTest
{
    [SetUpFixture]
    public partial class EFMultiTenantTestFixture
    {
        [OneTimeSetUp]
        public void SetDevelopmentEnvironmentVariables()
        {

        }

        [OneTimeSetUp]
        public void SetupDatabases()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFMultiTenantDbContext, Configuration>());
            Database.Deleter.DeleteAllData();
        }

        [OneTimeTearDown]
        public void RestoreDatabaseToInitialState()
        {
            using (var context = Database.DbContext)
            {
                context.SaveChanges();
            }
        }
    }
}