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
    public class UserControllerTests
    {
        private UserController controller;
        private IQueryable<User> users;

        private TestDBContext FormContext()
        {
            users = new UserCollection().Users;

            var userDbSet = new Mock<DbSet<User>>();
            TestHelper.InitDbSet(userDbSet, users);

            var context = new Mock<TestDBContext>();
            context.Setup(x => x.Users).Returns(userDbSet.Object);

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
        public void AddUserEmptyNameCheck() => AddUserBadRequestObjectResultCheck("", 1,  "Заполните данные");

        [DataTestMethod]
        [DataRow("Karl", 1)]
        [DataRow("Vasia", 1)]
        public void AddUserNameDuplicationCheck(string name, int roleid) => AddUserBadRequestObjectResultCheck(name, roleid, "Роль не существует");

        [DataTestMethod]
        [DataRow("Petia", 0)]
        [DataRow("Fedia", 0)]
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
            Assert.IsTrue(values.Count == 2);
            Assert.AreEqual(values[0].Name, users.First().Name);
            Assert.AreEqual(values[1].Name, users.Last().Name);
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(20)]
        [DataRow(555)]
        [DataRow(1000)]
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
        public void DeleteUserSuccess(int userId)
        {
            var result = controller.DeleteUser(userId);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void EditUserNotSet(int id, string name, int roleid )
        {
            var userExtendedDto = new UserExtendedDto(id, name, roleid);

            var result = controller.ChangeUser(userExtendedDto);

            Assert.IsTrue(result is BadRequestObjectResult);
            var value = (result as BadRequestObjectResult)?.Value as string;

            Assert.IsNotNull(value);
            Assert.AreEqual("Не задана изменяемая роль/имя", value);
        }

        [DataTestMethod]
        [DataRow(11230, "первая")]
        [DataRow(55, "test")]
        [DataRow(666, "second")]
        public void EditUserNotFound(int id, string name, int roleId)
        {
            var userExtendedDto = new UserExtendedDto(id, name, roleId);
            var result = controller.ChangeUser(userExtendedDto);

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
