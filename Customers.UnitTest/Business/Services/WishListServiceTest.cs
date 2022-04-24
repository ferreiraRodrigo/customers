using Customers.Business.Models;
using Customers.Business.Repositories;
using Customers.Business.Services;
using Customers.Infra.Adapters;
using Customers.Infra.Adapters.Product;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Customers.UnitTest.Business.Services
{
    public class WishListServiceTest
    {
        private readonly Mock<ICustomerRepository> _customerRepository;
        private readonly Mock<IWishListRepository> _wishlistRepository;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IProductAdapter> _productAdapter;
        
        private readonly WishListService _wishListService;

        private readonly Customer _customer;
        private readonly List<Product> _products;
        private readonly WishList _wishList;
        private readonly ProductDTO _productDTO;
        
        public WishListServiceTest()
        {
            _customerRepository = new Mock<ICustomerRepository>();
            _wishlistRepository = new Mock<IWishListRepository>();
            _productRepository = new Mock<IProductRepository>();
            _productAdapter = new Mock<IProductAdapter>();

            _wishListService = new WishListService(_customerRepository.Object, 
                _wishlistRepository.Object, 
                _productRepository.Object, 
                _productAdapter.Object);

            _customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = It.IsAny<string>(),
                Email = "test@test.com"
            };

            _wishList = new WishList
            {
                Id = Guid.NewGuid(),
                CustomerId = _customer.Id
            };

            _products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    Title = It.IsAny<string>(),
                    Price = It.IsAny<float>(),
                    Image = It.IsAny<string>(),
                    WishListId = _wishList.Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),                    
                    Title =It.IsAny<string>(),
                    Price = It.IsAny<float>(),
                    Image = It.IsAny<string>(),
                    WishListId = _wishList.Id
                }
            };

            _wishList.Products = _products;

            _productDTO = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Title = It.IsAny<string>(),
                Price = It.IsAny<float>(),
                Image = It.IsAny<string>(),
                Brand = It.IsAny<string>()
            };
        }

        [Fact]
        public async Task GetCustomerWishListAsync_ShouldReturnWishList_WhenCustomerWishListFound()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync(_wishList);

            // Act
            var wishlist = await _wishListService.GetCustomerWishListAsync(_customer.Id);

            // Assert
            Assert.Null(wishlist.Error);
            Assert.Null(wishlist.ErrorMessage);
            Assert.NotNull(wishlist.Result);
            Assert.Equal(_wishList.Id, wishlist.Result.Id);
            Assert.Equal(_wishList.CustomerId, wishlist.Result.CustomerId);
            Assert.Equal(_wishList.Products.Count, wishlist.Result.Products.Count);
        }

        [Fact]
        public async Task GetCustomerWishListAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Customer)null);

            // Act
            var wishlist = await _wishListService.GetCustomerWishListAsync(_customer.Id);

            // Assert
            Assert.NotNull(wishlist.Error);
            Assert.NotNull(wishlist.ErrorMessage);
            Assert.Null(wishlist.Result);
        }

        [Fact]
        public async Task GetCustomerWishListAsync_ShouldReturnNull_WhenCustomerWishListDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync((WishList)null);

            // Act
            var wishlist = await _wishListService.GetCustomerWishListAsync(_customer.Id);

            // Assert
            Assert.NotNull(wishlist.Error);
            Assert.NotNull(wishlist.ErrorMessage);
            Assert.Null(wishlist.Result);
        }
        
        [Fact]
        public async Task AddProductToCustomerWishListAsync_ShouldReturnProduct_WhenProductAddedToWishList()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync(_wishList);
            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((Product)null);
            _productAdapter.Setup(x => x.GetProduct(It.IsAny<Guid>())).ReturnsAsync(_productDTO);

            // Act
            var product = await _wishListService.AddProductToCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.Null(product.Error);
            Assert.Null(product.ErrorMessage);
            Assert.NotNull(product.Result);
            Assert.Equal(_productDTO.Id, product.Result.ProductId);
            Assert.Equal(_productDTO.Title, product.Result.Title);
            Assert.Equal(_productDTO.Price, product.Result.Price);
            Assert.Equal(_productDTO.Image, product.Result.Image);
            Assert.Equal(_wishList.Id, product.Result.WishListId);
        }

        [Fact]
        public async Task AddProductToCustomerWishListAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Customer)null);

            // Act
            var product = await _wishListService.AddProductToCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }

        [Fact]
        public async Task AddProductToCustomerWishListAsync_ShouldReturnNull_WhenCustomerWishListDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync((WishList)null);

            // Act
            var product = await _wishListService.AddProductToCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }

        [Fact]
        public async Task AddProductToCustomerWishListAsync_ShouldReturnProduct_WhenProductAlreadyExistsInWishList()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync(_wishList);
            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(_products.First());

            // Act
            var product = await _wishListService.AddProductToCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());
            
            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.NotNull(product.Result);
            Assert.Equal(_products.First().ProductId, product.Result.ProductId);
        }

        [Fact]
        public async Task AddProductToCustomerWishListAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync(_wishList);
            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((Product)null);
            _productAdapter.Setup(x => x.GetProduct(It.IsAny<Guid>())).ReturnsAsync((ProductDTO)null);

            // Act
            var product = await _wishListService.AddProductToCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }
        
        [Fact]
        public async Task GetProductFromCustomerWishListAsync_ShouldReturnProduct_WhenProductExistsInWishList()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync(_wishList);
            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(_products.First());

            // Act
            var product = await _wishListService.GetProductFromCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.Null(product.Error);
            Assert.Null(product.ErrorMessage);
            Assert.NotNull(product.Result);
            Assert.Equal(_products.First().ProductId, product.Result.ProductId);
            Assert.Equal(_products.First().Title, product.Result.Title);
            Assert.Equal(_products.First().Price, product.Result.Price);
            Assert.Equal(_products.First().Image, product.Result.Image);
            Assert.Equal(_wishList.Id, product.Result.WishListId);
        }

        [Fact]
        public async Task GetProductFromCustomerWishListAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Customer)null);

            // Act
            var product = await _wishListService.GetProductFromCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }
        
        [Fact]
        public async Task GetProductFromCustomerWishListAsync_ShouldReturnNull_WhenCustomerWishListDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync((WishList)null);

            // Act
            var product = await _wishListService.GetProductFromCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }

        [Fact]
        public async Task GetProductFromCustomerWishListAsync_ShouldReturnNull_WhenProductDoesNotExistInWishList()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync(_wishList);
            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((Product)null);

            // Act
            var product = await _wishListService.GetProductFromCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }

        [Fact]
        public async Task DeleteProductFromCustomerWishListAsync_ShouldReturnProduct_WhenProductDeletedFromWishList()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync(_wishList);
            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(_products.First());

            // Act
            var product = await _wishListService.DeleteProductFromCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.Null(product.Error);
            Assert.Null(product.ErrorMessage);
            Assert.NotNull(product.Result);
            Assert.Equal(_products.First().ProductId, product.Result.ProductId);
            Assert.Equal(_products.First().Title, product.Result.Title);
            Assert.Equal(_products.First().Price, product.Result.Price);
            Assert.Equal(_products.First().Image, product.Result.Image);
            Assert.Equal(_wishList.Id, product.Result.WishListId);
        }

        [Fact]
        public async Task DeleteProductFromCustomerWishListAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Customer)null);

            // Act
            var product = await _wishListService.DeleteProductFromCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }

        [Fact]
        public async Task DeleteProductFromCustomerWishListAsync_ShouldReturnNull_WhenCustomerWishListDoesNotExist()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync((WishList)null);

            // Act
            var product = await _wishListService.DeleteProductFromCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }
        
        [Fact]
        public async Task DeleteProductFromCustomerWishListAsync_ShouldReturnNull_WhenProductDoesNotExistInWishList()
        {
            // Arrange
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_customer);
            _wishlistRepository.Setup(x => x.GetCustomerWishListAsync(It.IsAny<Guid>())).ReturnsAsync(_wishList);
            _productRepository.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((Product)null);

            // Act
            var product = await _wishListService.DeleteProductFromCustomerWishListAsync(_customer.Id, It.IsAny<Guid>());

            // Assert
            Assert.NotNull(product.Error);
            Assert.NotNull(product.ErrorMessage);
            Assert.Null(product.Result);
        }
    }
}
