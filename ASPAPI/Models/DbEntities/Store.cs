using ASPAPI.Abstract.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASPAPI.Models.DbEntities
{
    public class Store : IEntity {
#nullable disable
        public int Id { get; set; }
        public int storeCount { get; set; }

        [ForeignKey(nameof(Productid))]  
        public int Productid { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        
        //[ForeignKey(nameof(OrderItem))]
        //public int Orderitemid { get; set; }
        //[JsonIgnore]
        //public OrderItem OrderItem { get; set;}
#nullable restore
    }
    
}
