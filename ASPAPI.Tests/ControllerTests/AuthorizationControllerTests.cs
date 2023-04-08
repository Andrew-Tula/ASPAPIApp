using ASPAPI.Controllers;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using ASPAPI.Tests.Data;
using ASPAPI.Tests.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ASPAPI.Tests.ControllerTests {
    [TestClass]
    public class AuthorizationControllerTests {
        private AuthorizationController controller;

        private TestDBContext FormContext() {
            var users = new UserCollection().Users;
            var roles = new RoleCollection().Roles;
            var userTokens = new UserTokenCollection().Tokens;

            var userDbSet = new Mock<DbSet<User>>();
            TestHelper.InitDbSet(userDbSet, users);

            var roleDbSet = new Mock<DbSet<Role>>();
            TestHelper.InitDbSet(roleDbSet, roles);

            var userTokenDbSet = new Mock<DbSet<UserToken>>();
            TestHelper.InitDbSet(userTokenDbSet, userTokens);

            var context = new Mock<TestDBContext>();
            context.Setup(x => x.Users).Returns(userDbSet.Object);
            context.Setup(x => x.Roles).Returns(roleDbSet.Object);
            context.Setup(x => x.UserTokens).Returns(userTokenDbSet.Object);

            return context.Object;
        }

        [TestInitialize]
        public void Initialize() {
            var context = FormContext();

            var configuration = new Mock<IConfiguration>();

            var userRepository = new UserRepository(context);
            userRepository.InitDbSet(context.Users);

            var roleRepository = new RoleRepository(context);
            roleRepository.InitDbSet(context.Roles);

            var userTokenRepository = new UserTokeRepository(context);
            userTokenRepository.InitDbSet(context.UserTokens);

            controller = new AuthorizationController(configuration.Object, userRepository, roleRepository, userTokenRepository);

            //  вот здесь и происходит проверка авторизации 
            controller!.ControllerContext.HttpContext = new DefaultHttpContext {
                RequestServices = TestHelper.AuthenticationServiceMock(),
                User = TestHelper.GetClaimsPrincipal("1")
            };
        }

        [TestMethod]
        public void TestAuthorizationSuccess() {
            var result = controller.TestAuthorization();
            Assert.IsTrue(result is OkObjectResult);
        }

        [DataTestMethod]
        [DataRow(" ", "12345678", 1)]
        [DataRow("", "87654321", 2)]
        [DataRow("Ludovik", " ", 1)]
        [DataRow("Lenin", "", 2)]
        public void TestRegistrationDeniedByNamePassword(string name, string password, int roleId)
        {
            var registrationDto = new RegistrationDto(name, password, roleId);
            var result = controller.Registration(registrationDto);
            var value = (result as BadRequestObjectResult)?.Value as string;
            var expectedResult = "Необходимо указать логин и пароль";
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }

        [DataTestMethod]
        [DataRow("Karl", "456456456 ", 1)]
        [DataRow("Vasia", "123123131", 2)]
        public void TestRegistrationDeniedByDoudleName(string name, string password, int roleId)
        {
            var registrationDto = new RegistrationDto(name, password, roleId);
            var result = controller.Registration(registrationDto);
            var value = (result as BadRequestObjectResult)?.Value as string;
            var expectedResult = "Пользователь с таким названием уже зарегистрирован";
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }

        [DataTestMethod]
        [DataRow("Vilen", "456456456 ", -1)]
        [DataRow("Tuzik", "123123131", 15)]
        public void TestRegistrationDeniedByWrongRole(string name, string password, int roleId)
        {
            var registrationDto = new RegistrationDto(name, password, roleId);
            var result = controller.Registration(registrationDto);
            var value = (result as BadRequestObjectResult)?.Value as string;
            var expectedResult = "Роль не найдена";
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }

        [DataTestMethod]
        [DataRow("", "456456456 ")]
        [DataRow("Tuzik", "")]
        [DataRow(" ", "886456456 ")]
        [DataRow("Sharik", " ")]
        public void TestLoginDeniedByNamePassword(string name, string password)
        {
            var loginDto = new LoginDto(name, password);
            var result = controller.Login(loginDto);
            var value = (result as BadRequestObjectResult)?.Value as string;
            var expectedResult = "Необходимо указать логин и пароль";
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }

        [DataTestMethod]
        [DataRow("Sharik", "456456456")]
        [DataRow("Tuzik", "1123456789")]
        public void TestLoginDeniedByWrongName(string name, string password)
        {
            var loginDto = new LoginDto(name, password);
            var result = controller.Login(loginDto);
            var value = (result as BadRequestObjectResult)?.Value as string;
            var expectedResult = "Пользователь не найден";
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }

        [DataTestMethod]
        [DataRow("Karl", "456456456")]
        [DataRow("Vasia", "1123456789")]
        public void TestLoginDeniedByWrongPassword(string name, string password)
        {
            var loginDto = new LoginDto(name, password);
            var result = controller.Login(loginDto);
            var value = (result as BadRequestObjectResult)?.Value as string;
            var expectedResult = "Неверный пароль";
            Assert.IsNotNull(value);
            Assert.AreEqual(value, expectedResult);
        }


        [DataTestMethod]
        [DataRow("Ola", "12345678")]
        [DataRow("Mark", "123456789")]
        [DataRow("Gete", "1122334455")]
        public void TestLoginSuccess(string name, string password)
        {
            var loginDto = new LoginDto(name, password);
            var result = controller.Login(loginDto);
            Assert.IsTrue(result is OkObjectResult);
        }

    }
}
