using System;
using EFMultiTenant.Models;

namespace MultiTenantEF_ASP.Models
{
    internal class TenantContext
    {
        private static System.Object tenantClearLock = new System.Object();
        public static void ClearTenant()
        {
            lock (tenantClearLock)
            {
                System.Threading.Thread.FreeNamedDataSlot("Tenant" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
        }

        private static System.Object tenantSetLock = new System.Object();
        public static void SetTenant(Guid tenantGuid)
        {
            lock (tenantSetLock)
            {
                LocalDataStoreSlot lds =
                    System.Threading.Thread.GetNamedDataSlot("Tenant" +
                                                             System.Threading.Thread.CurrentThread.ManagedThreadId);
                System.Threading.Thread.SetData(lds, tenantGuid);
            }
        }

        private static System.Object tenantGetLock = new System.Object();
        public static Guid GetTenant()
        {
            lock (tenantGetLock)
            {
                LocalDataStoreSlot lds = System.Threading.Thread.GetNamedDataSlot("Tenant" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                var tenantGuid = (Guid)System.Threading.Thread.GetData(lds);
                if (tenantGuid == null)
                {
                    throw new Exception("A tenant has not been set for the current thread.");
                }
                return tenantGuid;

            }
        }
    }
}