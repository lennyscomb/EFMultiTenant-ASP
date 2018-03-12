using NUnit.Framework;

namespace EFMultiTenantTest
{
    public abstract class EFMultiTenantTest
    {
        [SetUp]
        public void SetUp()
        {
            EFMultiTenantTestFixture.Database.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            EFMultiTenantTestFixture.Database.TearDown();
        }
    }
}