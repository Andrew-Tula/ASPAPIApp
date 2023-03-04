using ASPAPI.Abstract.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models.DbEntities {
    public class UserToken: IEntity {
        public int Id { get; set; }

        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        public User User { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
    }
}
