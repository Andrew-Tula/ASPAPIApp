using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities {
    public class User {
#nullable disable
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(RoleId))]
        public int RoleId { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }

        [JsonIgnore]
        public List<Order> Orders { get; set; }
#nullable restore
    }
}
