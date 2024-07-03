using CustomersManagementAPI.Controllers;
using CustomersManagementAPI.Models;
using CustomersManagementAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomersManagementAPI.Tests
{
    public class CustomersControllerTest
    {
        private readonly CustomersController _controller;
        private readonly Mock<CustomersRepository> _repoMock;
        private readonly Mock<ILogger<CustomersController>> _loggerMock;

        public CustomersControllerTest()
        {
            _loggerMock = new Mock<ILogger<CustomersController>>();
            _repoMock = new Mock<CustomersRepository>();
            _controller = new CustomersController(_repoMock.Object, _loggerMock.Object);
        }


        [Fact]
        public void Add_ReturnsCreatedAtActionResult()
        {
            var customer = new Customer { Firstname = "Jan", Surname = "Kowalski" };

            var result = _controller.Add(customer) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(null, result.ActionName);
            Assert.IsType<Customer>(result.Value);
        }

        [Fact]
        public void Add_DoesNotAddWhenCustomerAlreadyExists()
        {
            var customer = new Customer { Firstname = "Zbigniew", Surname = "Makowski" };
            var customer2 = new Customer { Firstname = "Zbigniew", Surname = "Makowski" };

            var result = _controller.Add(customer) as CreatedAtActionResult;
            var result2 = _controller.Add(customer2) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.IsType<Customer>(result.Value);
            Assert.Null(result2);
        }

        [Fact]
        public void Remove_ReturnsNoContentResult()
        {
            var customer = new Customer { Firstname = "Zosia", Surname = "Samosia" };
            var actionResult = _controller.Add(customer) as CreatedAtActionResult;
            var cust = actionResult.Value as Customer;

            var result = _controller.Remove(cust.Id);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Remove_ReturnsNotFound()
        {
            var notExistingGuid = Guid.NewGuid();
            var customer = new Customer { Firstname = "Ben", Surname = "Jon" };
            
            _controller.Add(customer);
            var result = _controller.Remove(notExistingGuid);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ListAll_ReturnsCustomerList()
        {
            List<Customer> customers; 
            OkObjectResult? result;

            result = _controller.ListAll() as OkObjectResult;
            customers = Assert.IsType<List<Customer>>(result.Value);
            var initialCount = customers.Count();
            var customer = new Customer { Firstname = "Maria", Surname = "Konopnicka" };
            var customer2 = new Customer { Firstname = "Jan", Surname = "Brzechwa" };

            _controller.Add(customer);
            _controller.Add(customer2);

            result = _controller.ListAll() as OkObjectResult;
            customers = Assert.IsType<List<Customer>>(result.Value);
            var count = customers.Count();

            Assert.NotNull(result);
            Assert.IsType<List<Customer>>(result.Value);
            Assert.Equal(initialCount+2, count);
        }
    }
}