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
    }
}