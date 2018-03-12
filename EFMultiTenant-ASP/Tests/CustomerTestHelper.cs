using EFMultiTenant.Models;

namespace EFMultiTenantTest
{
    public class CustomerTestHelper
    {
        public static CustomerInputViewModel CreateAnonymousCustomer()
        {
            CustomerInputViewModel expectedCustomer;
            expectedCustomer = new CustomerInputViewModel();
            expectedCustomer.Name = StringTestHelper.GetRandomString();
            DomainFacade.CreateCustomer(expectedCustomer);
            return expectedCustomer;
        }
    }
}