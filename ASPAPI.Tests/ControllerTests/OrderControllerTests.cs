using ASPAPI.Controllers;
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

namespace ASPAPI.Tests.ControllerTests {
    [TestClass]
    public class OrderControllerTests {
        private OrderController controller;
        private IQueryable<Order> orders;

        private TestDBContext FormContext() {
            orders = new OrderCollection().Orders;
            var users = new UserCollection().Users;
            var products = new ProductCollection().Products;

            var orderDbSet = new Mock<DbSet<Order>>();
            TestHelper.InitDbSet(orderDbSet, orders);

            var userDbSet = new Mock<DbSet<User>>();
            TestHelper.InitDbSet(userDbSet, users);

            var productDbSet = new Mock<DbSet<Product>>();
            TestHelper.InitDbSet(productDbSet, products);

            var context = new Mock<TestDBContext>();
            context.Setup(x => x.Users).Returns(userDbSet.Object);
            context.Setup(x => x.Orders).Returns(orderDbSet.Object);
            context.Setup(x => x.Products).Returns(productDbSet.Object);

            return context.Object;
        }

        [TestInitialize]
        public void Initialize() {
            var context = FormContext();

            var orderRepository = new OrderRepository(context);
            orderRepository.InitDbSet(context.Orders);

            var userRepository = new UserRepository(context);
            userRepository.InitDbSet(context.Users);

            controller = new OrderController(orderRepository, userRepository);
        }

        private void AddOrderBadRequestObjectResultCheck(string name, int userid, string expectedResult) {
            var orderBaseDto = new OrderBaseDto(name, userid);
            var result = controller.AddOrder(orderBaseDto);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }

        [TestMethod]
        public void AddOrderEmptyNameCheck() => AddOrderBadRequestObjectResultCheck("", 1, "Заполните название / никнейм заказа");

        [TestMethod]
        public void AddOrderUserNotFoundCheck() => AddOrderBadRequestObjectResultCheck("Set15", 500, "Пользовательне найден");

        [DataTestMethod]
        [DataRow("Set1", 1)]
        [DataRow("Set2", 1)]
        [DataRow("Snikers", 2)]
        [DataRow("Mars", 3)]
        public void AddOrderSucces(string name, int userid) {
            var orderBaseDto = new OrderBaseDto(name, userid);
            var result = controller.AddOrder(orderBaseDto);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void GetOrderSuccess() {
            var result = controller.GetOrder();
            Assert.IsTrue(result is OkObjectResult);
            var values = (result as OkObjectResult)?.Value as List<Order>;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count == 4);
            Assert.AreEqual(values[0].Name, orders.First().Name);
            Assert.AreEqual(values[3].Name, orders.Last().Name);
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(20)]
        [DataRow(555)]
        [DataRow(1000)]
        [DataRow(-2)]
        public void DeleteOrderNotFound(int id) {
            var result = controller.DeleteOrder(id);

            Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual("Такого заказа не существует", value);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void DeleteOrderSuccess(int id) {
            var result = controller.DeleteOrder(id);
            Assert.IsTrue(result is OkResult);
        }


        [DataTestMethod]
        [DataRow(1000, "Soup")]
        [DataRow(1101, "Cigarette")]
        [DataRow(2220, "Tabacco")]
        [DataRow(-10, "Wiskey")]

        public void EditOrderNotSet(int id, string name) {
            var orderDto = new OrderDto(id, name);

            var result = controller.EditOrder(orderDto);

            Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;

            Assert.IsNotNull(value);
            Assert.AreEqual("Такого заказа не существует", value);
        }

        [DataTestMethod]
        [DataRow(1, "")]
        [DataRow(2, "")]
        [DataRow(3, "")]
        [DataRow(4, "")]

        public void EditOrderNotSettedName(int id, string name) {
            var orderDto = new OrderDto(id, name);

            var result = controller.EditOrder(orderDto);

            Assert.IsTrue(result is BadRequestObjectResult);
            var value = (result as BadRequestObjectResult)?.Value as string;

            Assert.IsNotNull(value);
            Assert.AreEqual("Заполните название / никнейм заказа", value);
        }
    }
}
