using CarBank.WebApi.Data;
using CarBank.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBank.WebApi.Services
{
    public interface ICustomerService
    {
        Task<long?> CreateCustomer(Customer customer);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CustomerService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<long?> CreateCustomer(Customer customer)
        {
            var Customer  = await _applicationDbContext.Customers.AddAsync(customer);
            var result  = await _applicationDbContext.SaveChangesAsync();
            return Customer.Entity.Id;
        }
    }
}
