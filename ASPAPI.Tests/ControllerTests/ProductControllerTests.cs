using ASPAPI.Controllers;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using ASPAPI.Tests.Data;
using ASPAPI.Tests.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using System.Data;

namespace ASPAPI.Tests.ControllerTests
{
    [TestClass]
    public class ProductControllerTests
    {
        private ProductController controller;
        private IQueryable<Product> products;

        private TestDBContext FormContext()
        {
            products = new ProductCollection().Products;

            var productDbSet = new Mock<DbSet<Product>>();
            TestHelper.InitDbSet(productDbSet, products);

            var context = new Mock<TestDBContext>();
            context.Setup(x => x.Products).Returns(productDbSet.Object);

            return context.Object;
        }

        [TestInitialize]
        public void Initialize()
        {
            var context = FormContext();

            var productRepository = new ProductRepository(context);
            productRepository.InitDbSet(context.Products);

            controller = new ProductController(productRepository);
        }

        private void AddProductBadRequestObjectResultCheck(string name, string expectedResult)
        {
            var result = controller.AddProduct(name);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }

        [TestMethod]
        public void AddRoleEmptyNameCheck() => AddRoleBadRequestObjectResultCheck("", "Заполните данные");

        [DataTestMethod]
        [DataRow("админ")]
        [DataRow("пользователь")]
        public void AddRoleNameDuplicationCheck(string name) => AddRoleBadRequestObjectResultCheck(name, "Данная роль уже присутствует");

        [DataTestMethod]
        [DataRow("старший продавец")]
        [DataRow("кассир")]
        public void AddRoleSucces(string role)
        {
            var result = controller.AddRole(role);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void GetRolesSuccess()
        {
            var result = controller.GetRoles();

            Assert.IsTrue(result is OkObjectResult);
            var values = (result as OkObjectResult)?.Value as List<Role>;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count == 3);
            Assert.AreEqual(values[0].Name, roles.First().Name);
            Assert.AreEqual(values[2].Name, roles.Last().Name);
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(20)]
        [DataRow(555)]
        [DataRow(1000)]
        public void DeleteRoleNotFound(int roleId)
        {
            var result = controller.DeleteRole(roleId);

            Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual("Такой роли не существует", value);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void DeleteRoleSuccess(int roleId)
        {
            var result = controller.DeleteRole(roleId);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void EditRoleNotSet()
        {
            var roleDto = new RoleDto(1, "");

            var result = controller.EditRole(roleDto);

            Assert.IsTrue(result is BadRequestObjectResult);
            var value = (result as BadRequestObjectResult)?.Value as string;

            Assert.IsNotNull(value);
            Assert.AreEqual("Не задана изменяемая роль/имя", value);
        }

        [DataTestMethod]
        [DataRow(11230, "первая")]
        [DataRow(55, "test")]
        [DataRow(666, "second")]
        public void EditRoleNotFound(int roleId, string name)
        {
            var roleDto = new RoleDto(roleId, name);
            var result = controller.EditRole(roleDto);

            Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual("Такой роли не существует", value);
        }

        [DataTestMethod]
        [DataRow(1, "changed")]
        [DataRow(2, "changed 2")]
        public void EditRoleSuccess(int roleId, string name)
        {
            var roleDto = new RoleDto(roleId, name);
            var result = controller.EditRole(roleDto);

            Assert.IsTrue(result is OkResult);
        }
    }
}