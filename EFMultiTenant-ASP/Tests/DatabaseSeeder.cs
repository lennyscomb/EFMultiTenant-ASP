using EFMultiTenant.Models;

namespace EFMultiTenantTest
{
    public class DatabaseSeeder
    {
        public void Seed(EFMultiTenantDbContext dbContext)
        {
            dbContext.SaveChanges();
        }
    }
}