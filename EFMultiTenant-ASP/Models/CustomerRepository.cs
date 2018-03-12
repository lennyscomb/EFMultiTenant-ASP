using System;
using System.Linq;
using MultiTenantEF_ASP.Models;

namespace EFMultiTenant.Models
{
    public class CustomerRepository
    {
        public static void CreateCustomer(CustomerInputViewModel customerInputViewModel)
        {
            var customer = new Customer();
            customer.Name = customerInputViewModel.Name;
            TransactionManager.UnitOfWork().AddEntity(customer);
        }

        public static Customer FindCustomer(string name)
        {
            return TransactionManager.UnitOfWork().FindAll<Customer>().Where(a =>
            {
                return a.Name.Equals(name);
            }).FirstOrDefault();
        }
    }
}