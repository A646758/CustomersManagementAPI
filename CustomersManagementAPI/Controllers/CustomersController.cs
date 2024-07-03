using CustomersManagementAPI.Models;
using CustomersManagementAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomersManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomersRepository _repository;

        public CustomersController(ICustomersRepository repository, ILogger<CustomersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //Operation 1: Add a Customer
        [HttpPost]
        public IActionResult Add([FromBody] Customer customer)
        {
            #region Validation

            //here validation:
            //ModelState
            //Attributes in model (surname set as required)
            //if customer already exists - search by unique value
            //if values are correct
            //or e.g. FluentValidation

            //e.g.:
            var customers = _repository.GetAllCustomers().ToList();
            if (customers.Any(x => x.Firstname == customer.Firstname && x.Surname == customer.Surname))
            {
                _logger.LogInformation("Trying to add customer  with existing name and surname.");
                return BadRequest("A customer with the same name and surname already exists.");
            }

            #endregion Validation

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Adding customer failed: Model state is invalid");
                return BadRequest(ModelState);
            }

            customer.Id = Guid.NewGuid();
            try
            {
                _repository.AddCustomer(customer);
                _logger.LogInformation($"Successfully added customer {customer.Firstname} {customer.Surname}.");
                return CreatedAtAction(null, new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Adding customer {customer.Firstname} {customer.Surname} failed.");
                return StatusCode(500, "A server error occurred. Try again later or contact administrator.");
            }
        }


        //Operation 2: Remove a Customer, given their ID
        [HttpDelete("{id}")]
        public IActionResult Remove(Guid id)
        {
            var customers = _repository.GetAllCustomers();
            var customer = customers.FirstOrDefault(x => x.Id == id);
            if (customer == null)
            {
                _logger.LogWarning($"Remove customer action failed. Customer with Id {id} not found.");
                return NotFound();
            }

            try
            {
                _repository.DeleteCustomer(id);
                _logger.LogInformation($"Customer with Id {id} removed successfully");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Removing customer {customer.Firstname} {customer.Surname} failed.");
                return StatusCode(500, "A server error occurred. Try again later or contact administrator.");
            }
        }

        //Operation 3: List all Customers
        [HttpGet]
        public IActionResult ListAll()
        {
            try
            {
                _logger.LogInformation("List all customers");
                return Ok(_repository.GetAllCustomers().ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Listing all customers failed.");
                return StatusCode(500, "A server error occurred. Try again later or contact administrator.");
            }
        }
    }
}
