using EFMultiTenant.Models;

namespace EFMultiTenantTest
{
    public partial class EFMultiTenantTestFixture
    {
        /// <summary>
        /// Utility clsas for managing database objects.
        /// </summary>
        public static class Database
        {

            // https://msdn.microsoft.com/en-gb/data/hh949853.aspx?f=255&MSPPError=-2147217396#9
            // db contexts are supposed to be short lived and discarded. avoid having context for duration of more than one request or test

            private static EFMultiTenantDbContext _dbContext;
            public static EFMultiTenantDbContext DbContext => _dbContext ?? (_dbContext = EFMultiTenantDbContext.Create());
            public static DatabaseDeleter Deleter { get; internal set; }
            public static DatabaseSeeder Seeder { get; internal set; }


            static Database()
            {
                Deleter = new DatabaseDeleter();
                Seeder = new DatabaseSeeder();
            }


            public static void SetUp()
            {
                Seeder.Seed(DbContext);
            }

            public static void TearDown()
            {
                _dbContext = null;
                Deleter.DeleteAllData();
            }
        }
    }
}
