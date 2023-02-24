using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data;

public class UserCollection
{
    public IQueryable<User> Users { get; }

    public UserCollection()
    {
        Users = new List<User> {
            new User{ Id = 1, Name = "Karl" },
            new User{ Id = 2, Name = "Vasia" },
            new User{ Id = 3, Name = "Petia"},
            new User{ Id = 4, Name = "Fedia"}, 
        }.AsQueryable();
    }
}
