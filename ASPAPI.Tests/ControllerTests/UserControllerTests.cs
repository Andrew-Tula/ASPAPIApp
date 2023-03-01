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
    public class UserControllerTests
    {
        private UserController controller;
        private IQueryable<User> users;

        private TestDBContext FormContext()
        {
            users = new UserCollection().Users;
            var roles = new RoleCollection().Roles;

            var userDbSet = new Mock<DbSet<User>>();
            TestHelper.InitDbSet(userDbSet, users);

            var roleDbSet = new Mock<DbSet<Role>>();
            TestHelper.InitDbSet(roleDbSet, roles);

            var context = new Mock<TestDBContext>();
            context.Setup(x => x.Users).Returns(userDbSet.Object);
            context.Setup(x => x.Roles).Returns(roleDbSet.Object);

            return context.Object;
        }

        [TestInitialize]
        public void Initialize()
        {
            var context = FormContext();

            var userRepository = new UserRepository(context);
            userRepository.InitDbSet(context.Users);

            controller = new UserController(userRepository);
        }

        private void AddUserBadRequestObjectResultCheck(string name, int roleId, string expectedResult)
        {
            var userdto = new UserDto(name, roleId);
            var result = controller.AddUser(userdto);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }

        [TestMethod]
        public void AddUserEmptyNameCheck() => AddUserBadRequestObjectResultCheck("", 1, "Заполните данные");


        [TestMethod]
        [DataTestMethod]
        [DataRow("gaagg", 1)]
        [DataRow("Fedia", 2)]
        [DataRow("Vova", 3)]
        public void AddUserSucces(string name, int roleId)
        {
            var userdto = new UserDto(name, roleId);
            var result = controller.AddUser(userdto);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void GetUserSuccess()
        {
            var result = controller.GetUsers();
            Assert.IsTrue(result is OkObjectResult);
            var values = (result as OkObjectResult)?.Value as List<User>;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count == 4);
            Assert.AreEqual(values[0].Name, users.First().Name);
            Assert.AreEqual(values[3].Name, users.Last().Name);
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(20)]
        [DataRow(555)]
        [DataRow(1000)]
        [DataRow(-2)]
        public void DeleteUserNotFound(int id)
        {
            var result = controller.DeleteUser(id);

            Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual("Такого пользователя не существует", value);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void DeleteUserSuccess(int userId)
        {
            var result = controller.DeleteUser(userId);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        [DataRow(1, "", 1)]
        [DataRow(1, "", 2)]
        [DataRow(0, "", 1)]
        [DataRow(0, "", 2)]

        public void EditUserNotSet(int id, string name, int roleid)
        {
            var userExtendedDto = new UserExtendedDto(id, name, roleid);

            var result = controller.ChangeUser(userExtendedDto);

            Assert.IsTrue(result is BadRequestObjectResult);
            var value = (result as BadRequestObjectResult)?.Value as string;

            Assert.IsNotNull(value);
            Assert.AreEqual("Вы не ввели Имя пользователя для изменения", value);
        }

        [DataTestMethod]
        [DataRow(-1, "Karl", 1)]
        [DataRow(100500, "Vasia", 2)]
        [DataRow(99999, "Petia", 2)]
        [DataRow(00, "Fritz", 1)]
        public void EditUserNotFound(int id, string name, int roleId)
        {
            var userExtendedDto = new UserExtendedDto(id, name, roleId);
            var result = controller.ChangeUser(userExtendedDto);

            Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual("Такого пользователя не существует", value);
        }

        [DataTestMethod]
        [DataRow(1, "BigBoss", -100)]
        [DataRow(2, "Vasia", 500)]
        [DataRow(3, "Einstein", 00)]
        public void EditUserHasNotSettedRole(int id, string name, int roleId)
        {
            var userExtendedDto = new UserExtendedDto(id, name, roleId);
            var result = controller.ChangeUser(userExtendedDto);
            // Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;

            Assert.IsNotNull(value);
            // Assert.AreEqual("Роль не существует", value);
        }

        [DataTestMethod]
        [DataRow(2, "Chuvak", 2)]
        [DataRow(1, "Somebody", 1)]
        public void EditUserSuccess(int id, string name, int roleId)
        {
            var userExtendedDto = new UserExtendedDto(id, name, roleId);
            var result = controller.ChangeUser(userExtendedDto);

            Assert.IsTrue(result is OkResult);
        }
    }
}
