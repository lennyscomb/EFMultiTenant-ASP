using System;
using System.ComponentModel.DataAnnotations;

namespace EFMultiTenant.Models
{
    public class Customer : IEntity, ISecuredByTenant
    {
        [Key]
        public int Id { get; private set; }

        public String Name { get; set; }

        public Guid? SecuredByTenantId { get; set; }
    }
}