using ASPAPI.Models.DbEntities;

namespace ASPAPI.Tests.Data {
    public class UserTokenCollection {
        public IQueryable<UserToken> Tokens { get; }

        public UserTokenCollection() {
            Tokens = new List<UserToken> {
                new UserToken {
                    Id = 1,
                    CreationDate = DateTime.Now,
                    RefreshToken = "aaaaaaaaa",
                    UserId = 1,
                },
                new UserToken {
                    Id = 2,
                    CreationDate = DateTime.Now.AddMinutes(-1),
                    RefreshToken = "bbbbbbbbbbb",
                    UserId = 2,
                },
            }.AsQueryable();
        }
    }
}
