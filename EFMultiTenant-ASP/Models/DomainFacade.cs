using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiTenantEF_ASP.Models;

namespace EFMultiTenant.Models
{

    public class DomainFacade
    {
        public static CustomerViewModel FindCustomer(String name)
        {
            return TransactionManager.Execute(System.Reflection.MethodBase.GetCurrentMethod().Name, () =>
            {
                return new CustomerAssembler().Assemble(CustomerRepository.FindCustomer(name));
            });

        }

        public static void CreateCustomer(CustomerInputViewModel customerInputViewModel)
        {
            TransactionManager.Execute(System.Reflection.MethodBase.GetCurrentMethod().Name, () =>
            {
                CustomerRepository.CreateCustomer(customerInputViewModel);
                return null;
            });
        }

        public static List<CustomerViewModel> FindCustomers()
        {
            return TransactionManager.Execute(System.Reflection.MethodBase.GetCurrentMethod().Name, () =>
            {
                return new CustomerAssembler().Assemble(CustomerRepository.FindCustomers());
            });
        }
    }
}
