using CustomersManagementAPI.Models;

namespace CustomersManagementAPI.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private List<Customer> _customers;

        public CustomersRepository()
        {
            _customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), Firstname = "Zbigniew", Surname = "Kowalski" },
                new Customer { Id = Guid.NewGuid(), Firstname = "Jan", Surname = "Nowak" }
            };
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);
        }

        public void DeleteCustomer(Guid id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                _customers.Remove(customer);
            }
        }
    }
}
