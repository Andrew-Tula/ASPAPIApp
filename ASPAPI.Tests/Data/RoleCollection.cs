using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data;

public class RoleCollection {
    public IQueryable<Role> Roles { get; }

	public RoleCollection() {
        Roles = new List<Role> {
            new Role{ Id = 1, Name = "админ" },
            new Role{ Id = 2, Name = "пользователь" },
            new Role{ Id = 3, Name = "продавец" },
        }.AsQueryable();
    }
}
