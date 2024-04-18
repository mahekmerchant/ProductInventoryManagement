using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductInventoryManagement.Controllers;
using ProductInventoryManagement.Entities;
using ProductInventoryManagement.Exceptions;
using ProductInventoryManagement.Model;
using ProductInventoryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductInventoryManagementTests.Controller
{
    public class ProductInventoryManagementControllerTests
    {
        [Fact]
        public void GetAllProductsFromInventory_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);

            var fakeProducts = new List<Product>
            {
                new Product { Id=1,Name="Laptop",Brand="HP",Price=1500 },
                new Product { Id=2,Name="Mouse",Brand="Dell",Price=50 }
            };

            mockRepo.Setup(repo => repo.GetAllProductsFromInventory()).Returns(fakeProducts);

            // Act
            var result = controller.GetAllProductsFromInventory();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Product>>(actionResult.Value);
            Assert.Equal(fakeProducts.Count, returnValue.Count);
        }

        [Fact]
        public void GetProductFromInventoryById_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            var testProductId = 1;
            var fakeProduct = new Product { Id = 1, Name = "Laptop", Brand = "HP", Price = 1500 };

            mockRepo.Setup(repo => repo.GetProductFromInventoryById(testProductId)).Returns(fakeProduct);

            // Act
            var result = controller.GetProductFromInventoryById(testProductId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(fakeProduct, returnValue);
        }

        [Fact]
        public void GetProductFromInventoryById_ThrowsNotFoundException_WhenProductNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            var testProductId = 1;

            mockRepo.Setup(repo => repo.GetProductFromInventoryById(testProductId)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => controller.GetProductFromInventoryById(testProductId));
            Assert.Equal("Not Found Error: The product with the given Id was not found in the inventory.", exception.Message);
        }

        [Fact]
        public void GetProductsFromInventoryUsingPaging_ThrowsValidationException_WhenParametersAreNull()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => controller.GetProductsFromInventoryUsingPaging(null));
            Assert.Equal("Null Parameter Error: The filter or paging parameters cannot be null", exception.Message);
        }

        [Fact]
        public void GetProductsFromInventoryUsingPaging_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            var fakePagingParameters = new ProductInventoryManagementPagingParameters { PageNumber=1,PageSize=10 };
            var fakeProducts = new List<Product>
            {
                new Product { Id=1,Name="Laptop",Brand="HP",Price=1500 },
                new Product { Id=2,Name="Mouse",Brand="Dell",Price=50 }
            };

            mockRepo.Setup(repo => repo.GetProductsFromInventoryUsingPaging(fakePagingParameters)).Returns(fakeProducts);

            // Act
            var result = controller.GetProductsFromInventoryUsingPaging(fakePagingParameters);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Product>>(actionResult.Value);
            Assert.Equal(fakeProducts.Count, returnValue.Count);
        }

        [Fact]
        public void GetProductsFromInventoryUsingFilter_ThrowsValidationException_WhenParametersAreNull()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => controller.GetProductsFromInventoryUsingFilter(null, null));
            Assert.Equal("Null Parameter Error: The filter or paging parameters cannot be null", exception.Message);
        }

        [Fact]
        public void GetProductsFromInventoryUsingFilter_ReturnsOkResult_WithFilteredProducts()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            var fakeFilterParameters = new ProductInventoryMangementFilter { Name = "Mouse", Brand = "HP", MinPrice = 50, MaxPrice = 500 };
            var fakePagingParameters = new ProductInventoryManagementPagingParameters { PageNumber = 1, PageSize = 10 };
            var fakeProducts = new List<Product>
            {
                new Product { Id=1,Name="Mouse",Brand="HP",Price=50 },
                new Product { Id=2,Name="Mouse23",Brand="HP",Price=400 },
            };

            mockRepo.Setup(repo => repo.GetProductsFromInventoryUsingFilter(fakeFilterParameters, fakePagingParameters)).Returns(fakeProducts);

            // Act
            var result = controller.GetProductsFromInventoryUsingFilter(fakeFilterParameters, fakePagingParameters);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Product>>(actionResult.Value);
            Assert.Equal(fakeProducts.Count, returnValue.Count);
        }

        [Fact]
        public void AddProductToInventory_ThrowsValidationException_WhenProductIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => controller.AddProductToInventory(null));
            Assert.Equal("Null Object Error: The product cannot be null", exception.Message);
        }

        [Fact]
        public void AddProductToInventory_ThrowsValidationException_WhenDuplicateProduct()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            var testProduct = new Product { Id = 1, Name = "Laptop", Brand = "HP", Price = 1500 };
            var existingProducts = new List<Product>
             {
                testProduct
           
             };
            mockRepo.Setup(repo => repo.GetAllProductsFromInventory()).Returns(existingProducts);
            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => controller.AddProductToInventory(testProduct));
            Assert.Equal("Duplication Error: The product with same name and brand already exists in the inventory.", exception.Message);
        }

        [Fact]
        public void AddProductToInventory_ReturnsCreatedAtActionResult_WithCorrectData()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            var testProduct = new Product { Id = 1, Name = "Laptop", Brand = "HP", Price = 1500 };
            mockRepo.Setup(repo => repo.GetAllProductsFromInventory()).Returns(new List<Product>());
            // Act
            var result = controller.AddProductToInventory(testProduct);
            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.GetProductFromInventoryById), actionResult.ActionName);
            Assert.Equal(testProduct.Id, actionResult.RouteValues["id"]);
            var returnValue = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(testProduct, returnValue); // Ensure that the returned product is the one we added
        }

        [Fact]
        public void UpdateProductInsideInventory_ThrowsValidationException_WhenProductIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => controller.UpdateProductInsideInventory(1, null));
            Assert.Equal("Null Object Error: The product cannot be null", exception.Message);
        }

        [Fact]
        public void UpdateProductInsideInventory_ThrowsValidationException_WhenIdDoesNotMatchProductId()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            var testProduct = new Product { Id = 2, Name = "Laptop", Brand = "HP", Price = 1500 };
            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => controller.UpdateProductInsideInventory(1, testProduct));
            Assert.Equal("Invalid Id Error: The provided Id does not match the product's Id", exception.Message);
        }

        [Fact]
        public void UpdateProductInsideInventory_ThrowsNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            mockRepo.Setup(repo => repo.GetProductFromInventoryById(It.IsAny<int>())).Returns((Product)null);
            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => controller.UpdateProductInsideInventory(1, new Product { Id = 1 /* Initialize with test data */ }));
            Assert.Equal("Not Found Error: The product with the given Id was not found in the inventory.", exception.Message);
        }

        [Fact]
        public void UpdateProductInsideInventory_ReturnsNoContentResult_WhenValidExistingProduct()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);
            var testProduct = new Product { Id = 1, Name = "Laptop", Brand = "HP", Price = 1500 };

            mockRepo.Setup(repo => repo.GetProductFromInventoryById(testProduct.Id)).Returns(testProduct);

            // Act
            var result = controller.UpdateProductInsideInventory(testProduct.Id, testProduct);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteProductFromInventory_ThrowsNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);

            mockRepo.Setup(repo => repo.GetProductFromInventoryById(It.IsAny<int>())).Returns((Product)null);
            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => controller.DeleteProductFromInventory(1));
            Assert.Equal("Not Found Error: The product with the given Id was not found in the inventory.", exception.Message);
        }

        [Fact]
        public void DeleteProductFromInventory_ReturnsNoContentResult_WhenValidExistingProduct()
        {
            // Arrange
            var mockRepo = new Mock<IProductInventoryManagementRepository>();
            var mockLogger = new Mock<ILogger<ProductInventoryManagementController>>();
            var controller = new ProductInventoryManagementController(mockRepo.Object, mockLogger.Object);

            mockRepo.Setup(repo => repo.GetProductFromInventoryById(It.IsAny<int>())).Returns(new Product { Id = 1 /* Initialize with test data */ });

            // Act
            var result = controller.DeleteProductFromInventory(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
