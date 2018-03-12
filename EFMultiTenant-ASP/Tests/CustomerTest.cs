using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using EFMultiTenant.Models;
using FluentAssertions;
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
            actualCustomer.Should().NotBeNull();
            actualCustomer.Name.Should().BeEquivalentTo(expectedCustomer.Name);
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
            customers.Should().NotBeEmpty().And.Contain(x => x.Name.Equals(secondTenantCustomer.Name));
        }

        [Test]
        public void FindCustomers_TenantFilteringDisabled()
        {
            // assemble
            TenantContext.SetTenantToMaster();

            var firstTenantCustomer = CustomerTestHelper.CreateAnonymousCustomer();
            var secondTenantCustomer = CustomerTestHelper.CreateAnonymousCustomer();


            // act
            List<CustomerViewModel> customers = DomainFacade.FindCustomers();

            // assert
            customers.Should().NotBeEmpty().And.HaveCount(2);
            customers.Should().Contain(x => x.Name.Equals(firstTenantCustomer.Name));
            customers.Should().Contain(x => x.Name.Equals(secondTenantCustomer.Name));
        }
    }
}
