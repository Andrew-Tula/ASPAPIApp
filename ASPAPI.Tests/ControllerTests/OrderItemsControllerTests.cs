using ASPAPI.Controllers;
using ASPAPI.Migrations;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using ASPAPI.Tests.Data;
using ASPAPI.Tests.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data;
using System.Diagnostics;

namespace ASPAPI.Tests.ControllerTests
{
    [TestClass]
    public class OrderItemControllerTests
    {
        private OrderItemController controller;
        private IQueryable<OrderItem> orderItems;

        private TestDBContext FormContext()
        {
            orderItems = new OrderItemCollection().OrderItems;
            var orders = new OrderCollection().Orders;
            var users = new UserCollection().Users;
            var products = new ProductCollection().Products;
            var storeProducts = new StoreProductCollection().StoreProducts;

            var orderItemsDbSet = new Mock<DbSet<OrderItem>>();
            TestHelper.InitDbSet(orderItemsDbSet, orderItems);

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

            var storeProductRepository = new StoreProductRepository(context);
            storeProductRepository.InitDbSet(context.StoreProducts);


            controller = new OrderItemController(orderItemRepository, storeProductRepository);
        }

        private void AddOrderItemBadRequestObjectResultCheck(OrderItemDto data, string expectedResult)
        {
            var result = controller.AddOrderItem(data);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }

        [TestMethod]
        public void AddOrderItemEmptyDataCheck()
        {
            AddOrderItemBadRequestObjectResultCheck(null, "Пустое поле данных");
        }

        [DataTestMethod]
        [DataRow(0, 1, 2)]
        [DataRow(-1, 1, 2)]
        [DataRow(-105, 2, 1)]
        public void AddOrderUserItemNotFoundCheck(int productCount, int storeProductId, int orderid)
        {
            var orderItemDto = new OrderItemDto(productCount, storeProductId, orderid);
            var result = controller.AddOrderItem(orderItemDto);
            var expectedResult = "Количество товара не может быть нулевым или отрицательным";
            Assert.AreEqual(result, expectedResult);
        }

        [DataTestMethod]
        [DataRow(1, 0, 1)]
        [DataRow(1, 9999999, 2)]
        public void ProductDoesNotExist(int productCount, int storeProduct, int orderId)
        {
            var orderItemDto = new OrderItemDto(productCount, storeProduct, orderId);
            var result = controller.AddOrderItem(orderItemDto);
            var expectedResult = "Продукт не существует";
            Assert.AreEqual(result, expectedResult);
        }

        // =====================   ??????????????????????????????????    ===============================
        //    if (storeProduct.StoreCount<data.productCount)
        //    return BadRequest("Нет достаточного количества продукта в магазине");

        [DataTestMethod]
        [DataRow(1, 1, 0)]
        [DataRow(1, 2, 900000)]
        public void OrderDoesNotExist(int productCount, int storeProduct, int orderId)
        {
            var orderItemDto = new OrderItemDto(productCount, storeProduct, orderId);
            var result = controller.AddOrderItem(orderItemDto);
            var expectedResult = "Заказ не существует";
            Assert.AreEqual(result, expectedResult);
        }



        [TestMethod]
        public void GetOrderItemSuccess()
        {
            var result = controller.GetOrderItem();
            Assert.IsTrue(result is OkObjectResult);
            var values = (result as OkObjectResult)?.Value as List<OrderItem>;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count == 4);
            Assert.AreEqual(values[0].ProductCount, orderItems.First().ProductCount);
            Assert.AreEqual(values[3].ProductCount, orderItems.Last().ProductCount);
        }

        //[DataTestMethod]
        //[DataRow(10)]
        //[DataRow(20)]
        //[DataRow(555)]
        //[DataRow(1000)]
        //[DataRow(-2)]
        //public void DeleteOrderNotFound(int id)
        //{
        //    var result = controller.DeleteOrder(id);

        //    Assert.IsTrue(result is NotFoundObjectResult);
        //    var value = (result as NotFoundObjectResult)?.Value as string;
        //    Assert.IsNotNull(value);
        //    Assert.AreEqual("Такого заказа не существует", value);
        //}

        //[DataTestMethod]
        //[DataRow(1)]
        //[DataRow(2)]
        //[DataRow(3)]
        //public void DeleteOrderSuccess(int id)
        //{
        //    var result = controller.DeleteOrder(id);
        //    Assert.IsTrue(result is OkResult);
        //}


        //[TestMethod]
        //[DataRow(1000, "Soup")]
        //[DataRow(1101, "Cigarette")]
        //[DataRow(2220, "Tabacco")]
        //[DataRow(-10, "Wiskey")]

        //public void EditOrderNotSet(int id, string name)
        //{
        //    var orderDto = new OrderDto(id, name);

        //    var result = controller.EditOrder(orderDto);

        //    Assert.IsTrue(result is NotFoundObjectResult);
        //    var value = (result as NotFoundObjectResult)?.Value as string;

        //    Assert.IsNotNull(value);
        //    Assert.AreEqual("Такого заказа не существует", value);
        //}

        //[TestMethod]
        //[DataRow(1, "")]
        //[DataRow(2, "")]
        //[DataRow(3, "")]
        //[DataRow(4, "")]

        //public void EditOrderNotSettedName(int id, string name)
        //{
        //    var orderDto = new OrderDto(id, name);

        //    var result = controller.EditOrder(orderDto);

        //    Assert.IsTrue(result is BadRequestObjectResult);
        //    var value = (result as BadRequestObjectResult)?.Value as string;

        //    Assert.IsNotNull(value);
        //    Assert.AreEqual("Заполните название / никнейм заказа", value);
        //}
    }
}
