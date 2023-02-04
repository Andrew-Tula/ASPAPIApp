using Microsoft.EntityFrameworkCore;
using Moq;

namespace ASPAPI.Tests.Services;
public class TestHelper {
    public static void InitDbSet<T>(Mock<DbSet<T>> mockDbSet, IQueryable<T> entities) where T : class {
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
    }
}
