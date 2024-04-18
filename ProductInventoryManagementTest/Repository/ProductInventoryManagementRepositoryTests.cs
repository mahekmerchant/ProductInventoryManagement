//using Microsoft.EntityFrameworkCore;
//using Moq;
//using ProductInventoryManagement.Data;
//using ProductInventoryManagement.Entities;
//using ProductInventoryManagement.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace ProductInventoryManagementTests.Repository
//{
//    public class ProductInventoryManagementRepositoryTests
//    {
//        [Fact]
//        public void GetAllProductsFromInventory_ReturnsAllProducts()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<ProductInventoryManagementDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            using (var context = new ProductInventoryManagementDbContext(options,true))
//            {
//                context.Products.Add(new Product { Id = 4, Name = "Laptop", Brand = "HP", Price = 1500 });
//                context.Products.Add(new Product { Id = 5, Name = "Mouse", Brand = "Dell", Price = 50 });
//                context.SaveChanges();
//            }

//            // Act
//            List<Product> result;
//            using (var context = new ProductInventoryManagementDbContext(options, true))
//            {
//                var repository = new ProductInventoryManagementRepository(context);
//                result = repository.GetAllProductsFromInventory();
//            }

//            // Assert
//            Assert.Equal(2, result.Count);
//        }

//        [Fact]
//        public void GetProductFromInventoryById_ReturnsCorrectProduct_WhenValidId()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<ProductInventoryManagementDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            var testProduct = new Product { Id = 4, Name = "Laptop", Brand = "HP", Price = 1500 };

//            using (var context = new ProductInventoryManagementDbContext(options, true))
//            {
//                context.Products.Add(testProduct);
//                context.SaveChanges();
//            }

//            // Act
//            Product result;
//            using (var context = new ProductInventoryManagementDbContext(options, true))
//            {
//                var repository = new ProductInventoryManagementRepository(context);
//                result = repository.GetProductFromInventoryById(4);
//            }

//            // Assert
//            Assert.Equal(testProduct.Id, result.Id);
//        }

//        [Fact]
//        public void GetProductFromInventoryById_ReturnsNull_WhenInvalidId()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<ProductInventoryManagementDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            using (var context = new ProductInventoryManagementDbContext(options,true))
//            {
//                context.Products.Add(new Product { Id = 4, Name = "Laptop", Brand = "HP", Price = 1500 });
//                context.SaveChanges();
//            }

//            // Act
//            Product result;
//            using (var context = new ProductInventoryManagementDbContext(options, true))
//            {
//                var repository = new ProductInventoryManagementRepository(context);
//                result = repository.GetProductFromInventoryById(2); // ID does not exist
//            }

//            // Assert
//            Assert.Null(result);
//        }
//    }
//}
