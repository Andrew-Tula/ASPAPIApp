using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data;

public class UserCollection
{
    public IQueryable<User> Users { get; }

    public UserCollection()
    {
        Users = new List<User> {
            new User{ Id = 1, Name = "Karl", Salt = "", Hash = ""},
            new User{ Id = 2, Name = "Vasia", Salt = "", Hash = ""},
            new User{ Id = 3, Name = "Petia", Salt = "", Hash = ""},
            new User{ Id = 4, Name = "Fedia", Salt = "", Hash = ""}, 
            new User { Id = 8, Name = "Ola", Salt = "NGjU5FE=", Hash = "98E01462E3620D907372414E06EE3D3BAE784525"},
            new User { Id = 9, Name = "Mark", Salt = "vseFqLg=", Hash = "172CAC90FA8C0B6E9A0F403995F451AAF01126A9"} 
        }.AsQueryable();
    }
}
