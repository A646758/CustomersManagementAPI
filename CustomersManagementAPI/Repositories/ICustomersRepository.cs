using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomersManagementAPI.Models;

namespace CustomersManagementAPI.Repositories
{
    public interface ICustomersRepository
    {
        void AddCustomer(Customer customer);
        void DeleteCustomer(Guid id);
        IEnumerable<Customer> GetAllCustomers();
    }
}
