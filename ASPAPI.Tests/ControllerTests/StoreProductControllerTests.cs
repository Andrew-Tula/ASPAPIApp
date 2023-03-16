using ASPAPI.Controllers;
using ASPAPI.Migrations;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using ASPAPI.Tests.Data;
using ASPAPI.Tests.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data;
using System.Diagnostics;

namespace ASPAPI.Tests.ControllerTests
{
    [TestClass]
    public class StoreProductControllerTests
    {
        private StoreProductController controller;
        private IQueryable<StoreProduct> storeProducts;

        private TestDBContext FormContext()
        {

            var orderItems = new OrderItemCollection().OrderItems;
            var orders = new OrderCollection().Orders;
            var users = new UserCollection().Users;
            var products = new ProductCollection().Products;
            var storeProducts = new StoreProductCollection().StoreProducts;
            var stores = new StoreCollection().Stores;

            var orderItemsDbSet = new Mock<DbSet<OrderItem>>();
            TestHelper.InitDbSet(orderItemsDbSet, orderItems);

            var storeDbSet = new Mock<DbSet<Store>>();
            TestHelper.InitDbSet(storeDbSet, stores);

            var orderDbSet = new Mock<DbSet<Order>>();
            TestHelper.InitDbSet(orderDbSet, orders);

            var userDbSet = new Mock<DbSet<User>>();
            TestHelper.InitDbSet(userDbSet, users);

            var productDbSet = new Mock<DbSet<Product>>();
            TestHelper.InitDbSet(productDbSet, products);

            var storeProductDbSet = new Mock<DbSet<StoreProduct>>();
            TestHelper.InitDbSet(storeProductDbSet, storeProducts);

            var context = new Mock<TestDBContext>();
            context.Setup(x => x.Users).Returns(userDbSet.Object);
            context.Setup(x => x.Orders).Returns(orderDbSet.Object);
            context.Setup(x => x.Products).Returns(productDbSet.Object);
            context.Setup(x => x.OrderItems).Returns(orderItemsDbSet.Object);
            context.Setup(x => x.Stores).Returns(storeDbSet.Object);
            context.Setup(x => x.StoreProducts).Returns(storeProductDbSet.Object);

            return context.Object;
        }

        [TestInitialize]
        public void Initialize()
        {
            var context = FormContext();

            var orderRepository = new OrderRepository(context);
            orderRepository.InitDbSet(context.Orders);

            var userRepository = new UserRepository(context);
            userRepository.InitDbSet(context.Users);

            var productRepository = new ProductRepository(context);
            productRepository.InitDbSet(context.Products);

            var orderItemRepository = new OrderItemRepository(context);
            orderItemRepository.InitDbSet(context.OrderItems);

            var storeRepository = new StoreRepository(context);
            storeRepository.InitDbSet(context.Stores);

            var storeProductRepository = new StoreProductRepository(context);
            storeProductRepository.InitDbSet(context.StoreProducts);


              controller = new StoreProductController(storeProductRepository, storeRepository, productRepository);
          
        }

        //private void AddOrderItemBadRequestObjectResultCheck(OrderItemDto data, string expectedResult)
        //{
        //    var result = controller.AddStoreProduct(data);
        //    Assert.IsTrue(result is BadRequestObjectResult);

        //    var resultText = (result as BadRequestObjectResult)?.Value as string;
        //    Assert.AreEqual(expectedResult, resultText);
        //}

        //[TestMethod]
        //public void AddOrderItemEmptyDataCheck()
        //{
        //    AddOrderItemBadRequestObjectResultCheck(null, "Пустое поле данных");
        //}

        [DataTestMethod]
        [DataRow(0, 1, 1)]
        [DataRow(-1, 1, 1)]
        [DataRow(100500, 2, 1)]
        public void AddStoreProduct_NotTheShopCheck(int StoreId, int ProductId, int StoreCount)
        {
            var storeProductDTO = new StoreProductDTO(StoreId, ProductId, StoreCount);
            var result = controller.AddStoreProduct(storeProductDTO);
            var expectedResult = "Нет магазина";
            var value = (result as BadRequestObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }

        [DataTestMethod]
        [DataRow(1, 0, 1)]
        [DataRow(2, -2, 1)]
        [DataRow(3, 98989898, 1)]
        public void AddStoreProduct_ProductNotExistCheck(int StoreId, int ProductId, int StoreCount)
        {
            var storeProductDTO = new StoreProductDTO(StoreId, ProductId, StoreCount);
            var result = controller.AddStoreProduct(storeProductDTO);
            var expectedResult = "Нет продукта";
            var value = (result as BadRequestObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }

        [DataTestMethod]
        [DataRow(1, 2, 0)]
        [DataRow(2, 1, 0)]
        public void AddStoreProduct_StoreCountIsZeroCheck(int StoreId, int ProductId, int StoreCount)
        {
            var storeProductDTO = new StoreProductDTO(StoreId, ProductId, StoreCount);
            var result = controller.AddStoreProduct(storeProductDTO);
            var expectedResult = "Заполните количество товара / продукта";
            var value = (result as BadRequestObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }

        [DataTestMethod]
        [DataRow(1, 1, 2)]
        [DataRow(3, 1, 1)]
        [DataRow(2, 2, 1)]
        public void AddStoreProductSuccess(int productCount, int storeProduct, int orderId)
        {
            var storeProductDTO = new StoreProductDTO(productCount, storeProduct, orderId);
            var result = controller.AddStoreProduct(storeProductDTO);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void GetStoreProductSuccess()
        {
            var result = controller.GetStoreProduct();
            Assert.IsTrue(result is OkObjectResult);
            var values = (result as OkObjectResult)?.Value as List<StoreProduct>;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count == 4);
         //   Assert.AreEqual(values[0].StoreCount, storeProducts.First().StoreCount);
          //  Assert.AreEqual(values[3].StoreCount, storeProducts.Last().StoreCount);
        }


        [DataTestMethod]
        [DataRow(2, 2, 1)]
        [DataRow(1, 1, 2)]
        public void EditStoreProductSuccess(int productCount, int storeProduct, int orderId)
        {
            var storeProductDTO = new StoreProductDTO(productCount, storeProduct, orderId);
            var result = controller.EditStoreProduct(storeProductDTO);
            Assert.IsTrue(result is OkResult);
        }      
        





    }
}
