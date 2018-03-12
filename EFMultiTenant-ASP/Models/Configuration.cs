using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace EFMultiTenant.Models
{
    public class Configuration : DbMigrationsConfiguration<EFMultiTenantDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Symend.App.Database.EFMultiTenantDbContext";
        }
    }
}
