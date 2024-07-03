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
