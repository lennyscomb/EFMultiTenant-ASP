using System;
using System.Collections.Generic;
using System.Linq;
using EFMultiTenant.Models;
using FluentAssert;
using MultiTenantEF_ASP.Models;
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


        [Test]
        public void FindCustomers_MultipleTenants()
        {
            // assemble
            var firstTenant = TenantTestHelper.CreateAnonymousTenantId();
            var secondTenant = TenantTestHelper.CreateAnonymousTenantId();

            TenantContext.SetTenant(firstTenant);
            var firstTenantCustomer = CustomerTestHelper.CreateAnonymousCustomer();

            TenantContext.SetTenant(secondTenant);
            var secondTenantCustomer = CustomerTestHelper.CreateAnonymousCustomer();


            // act
            List<CustomerViewModel> customers = DomainFacade.FindCustomers();

            // assert
            customers.ShouldNotBeNull();
            customers.Count.ShouldBeEqualTo(1);
            customers.First().Name.ShouldBeEqualTo(secondTenantCustomer.Name);
        }
    }
}
