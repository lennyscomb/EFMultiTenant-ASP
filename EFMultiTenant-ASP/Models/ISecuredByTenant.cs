using System;

namespace EFMultiTenant.Models
{
    internal interface ISecuredByTenant
    {
        Guid? SecuredByTenantId { get; set; }
    }
}