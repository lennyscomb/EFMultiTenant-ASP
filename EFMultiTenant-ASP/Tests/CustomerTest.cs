using System;
using EFMultiTenant.Models;
using FluentAssert;
using NUnit.Framework;

namespace EFMultiTenantTest
{

    public class CustomerTest : EFMultiTenantTest
    {
        [Test]
        public void CreateCustomer()
        {
            // assemble
            CustomerInputViewModel expectedCustomer = new CustomerInputViewModel();
            expectedCustomer.Name = "TestCustomerName";

            // act
            DomainFacade.CreateCustomer(expectedCustomer);

            // assert
            CustomerViewModel actualCustomer = DomainFacade.FindCustomer(expectedCustomer.Name);
            actualCustomer.ShouldNotBeNull();
            actualCustomer.Name.ShouldBeEqualTo(expectedCustomer.Name);
        }
    }
}
