using MultiTenantEF_ASP.Models;
using NUnit.Framework;

namespace EFMultiTenantTest
{
    public abstract class EFMultiTenantTest
    {
        [SetUp]
        public void SetUp()
        {
            TenantContext.SetTenant(TenantTestHelper.CreateAnonymousTenantId());
            EFMultiTenantTestFixture.Database.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            TenantContext.ClearTenant();
            EFMultiTenantTestFixture.Database.TearDown();
        }
    }
}