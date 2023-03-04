using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace ASPAPI.Tests.Services;
public class TestHelper {
    public static void InitDbSet<T>(Mock<DbSet<T>> mockDbSet, IQueryable<T> entities) where T : class {
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
    }


    public static IServiceProvider AuthenticationServiceMock() {
        var authServiceMock = new Mock<IAuthenticationService>();
        authServiceMock.Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.FromResult((object?)null));

        authServiceMock.Setup(s => s.SignOutAsync(It.IsAny<HttpContext>(),
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    It.IsAny<AuthenticationProperties>())).
                    Returns(Task.FromResult(true));

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(s => s.GetService(typeof(IAuthenticationService))).Returns(authServiceMock.Object);
        return serviceProviderMock.Object;
    }

    public static ClaimsPrincipal GetClaimsPrincipal(string id) {
        var claims = new[] {
            new Claim("Id", id),
        };
        var identity = new ClaimsIdentity(claims);
        return new ClaimsPrincipal(identity);
    }
}
