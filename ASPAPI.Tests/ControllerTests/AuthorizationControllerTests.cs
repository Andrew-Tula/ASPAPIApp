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

        [TestMethod]
        public void TestAuthorizationDenied() 
        {

        }
    }
}
