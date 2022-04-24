using Customers.Business.Models;
using Customers.Business.Repositories;
using Customers.Business.Services.Interfaces;
using Customers.Presentation.Dtos;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Customers.UnitTest.Business.Services
{
    public class CustomerServiceTest
    {
        private readonly Mock<ICustomerRepository> _customerRepository;
        private readonly Mock<IWishListRepository> _wishlistRepository;

        private readonly CustomerService _customerService;
        
        private readonly Customer _customer;

        public CustomerServiceTest()
        {
            _customerRepository = new Mock<ICustomerRepository>();
            _wishlistRepository = new Mock<IWishListRepository>();

            _customerService = new CustomerService(_customerRepository.Object, 
                _wishlistRepository.Object);

            _customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = It.IsAny<string>(),
                Email = "test@test.com"
            };
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldReturnCustomer_WhenCustomerCreated()
        {
            // Arrange
            var customerDTO = new CustomerCreationDTO
            {
                Name = _customer.Name,
                Email = _customer.Email,
                Password = It.IsAny<string>()
            };

            _customerRepository.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync((Customer)null);
            _customerRepository.Setup(x => x.CreateAsync(It.IsAny<Customer>())).ReturnsAsync(_customer);

            // Act
            var customer = await _customerService.CreateCustomerAsync(customerDTO);

            // Assert
            Assert.Null(customer.Error);
            Assert.Null(customer.ErrorMessage);
            Assert.NotNull(customer.Result);
            Assert.Equal(_customer.Id, customer.Result.Id);
            Assert.Equal(_customer.Name, customer.Result.Name);
            Assert.Equal(_customer.Email, customer.Result.Email);
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldReturnExistingCustomer_WhenCustomerWithEmailAlreadyExists()
        {
            // Arrange
            var customerDTO = new CustomerCreationDTO
            {
                Name = _customer.Name,
                Email = _customer.Email,
                Password = It.IsAny<string>()
            };

            _customerRepository.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(_customer);

            // Act
            var customer = await _customerService.CreateCustomerAsync(customerDTO);

            // Assert
            Assert.NotNull(customer.Error);
            Assert.NotNull(customer.ErrorMessage);
            Assert.NotNull(customer.Result);
        }

        [Fact]
        public async Task GetCustomerAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = _customer.Id;
            _customerRepository.Setup(x => x.GetAsync(customerId)).ReturnsAsync(_customer);

            // Act
            var customer = await _customerService.GetCustomerAsync(customerId);


            // Assert
            Assert.Null(customer.Error);
            Assert.Null(customer.ErrorMessage);
            Assert.NotNull(customer);
            Assert.Equal(customerId, customer.Result.Id);
        }

        [Fact]
        public async Task GetCustomerAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _customerRepository.Setup(x => x.GetAsync(customerId)).ReturnsAsync((Customer)null);

            // Act
            var customer = await _customerService.GetCustomerAsync(customerId);

            // Assert
            Assert.NotNull(customer.Error);
            Assert.NotNull(customer.ErrorMessage);
            Assert.Null(customer.Result);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldReturnCustomer_WhenCustomerUpdated()
        {
            // Arrange
            var customerId = _customer.Id;
            var customerDTO = new CustomerUpdateDTO
            {
                Name = _customer.Name
            };

            _customerRepository.Setup(x => x.GetAsync(customerId)).ReturnsAsync(_customer);

            // Act
            var customer = await _customerService.UpdateCustomerAsync(customerDTO, customerId);

            // Assert
            Assert.Null(customer.Error);
            Assert.Null(customer.ErrorMessage);
            Assert.NotNull(customer.Result);
            Assert.Equal(_customer.Id, customer.Result.Id);
            Assert.Equal(_customer.Name, customer.Result.Name);
            Assert.Equal(_customer.Email, customer.Result.Email);
        }
        
        [Fact]
        public async Task UpdateCustomerAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customerDTO = new CustomerUpdateDTO
            {
                Name = _customer.Name
            };

            _customerRepository.Setup(x => x.GetAsync(customerId)).ReturnsAsync((Customer)null);

            // Act
            var customer = await _customerService.UpdateCustomerAsync(customerDTO, customerId);

            // Assert
            Assert.NotNull(customer.Error);
            Assert.NotNull(customer.ErrorMessage);
            Assert.Null(customer.Result);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldReturnCustomer_WhenCustomerDeleted()
        {
            // Arrange
            var customerId = _customer.Id;
            _customerRepository.Setup(x => x.GetAsync(customerId)).ReturnsAsync(_customer);

            // Act
            var customer = await _customerService.DeleteCustomerAsync(customerId);

            // Assert
            Assert.Null(customer.Error);
            Assert.Null(customer.ErrorMessage);
            Assert.NotNull(customer.Result);
            Assert.Equal(_customer.Id, customer.Result.Id);
            Assert.Equal(_customer.Name, customer.Result.Name);
            Assert.Equal(_customer.Email, customer.Result.Email);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _customerRepository.Setup(x => x.GetAsync(customerId)).ReturnsAsync((Customer)null);

            // Act
            var customer = await _customerService.DeleteCustomerAsync(customerId);

            // Assert
            Assert.NotNull(customer.Error);
            Assert.NotNull(customer.ErrorMessage);
            Assert.Null(customer.Result);
        }
    }
}
