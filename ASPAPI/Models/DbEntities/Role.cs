using ASPAPI.Abstract.Models;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities {
    public class Role: IEntity {
#nullable disable
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<User> Users { get; set; }
#nullable restore
    }
}
