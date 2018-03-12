using System;

namespace EFMultiTenantTest
{
    public class TenantTestHelper
    {
        public static Guid CreateAnonymousTenantId()
        {
            return Guid.NewGuid();
        }
    }
}