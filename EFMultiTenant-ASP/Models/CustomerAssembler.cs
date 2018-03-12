using System.Collections.Generic;

namespace EFMultiTenant.Models
{
    public class CustomerAssembler
    {
        public CustomerViewModel Assemble(Customer customer)
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();
            customerViewModel.Id = customer.Id;
            customerViewModel.Name = customer.Name;
            return customerViewModel;
        }

        public List<CustomerViewModel> Assemble(List<Customer> customers)
        {
            var customerViewModels = new List<CustomerViewModel>();
            foreach (var customer in customers)
            {
                customerViewModels.Add(Assemble(customer));
            }
            return customerViewModels;
        }
    }
}