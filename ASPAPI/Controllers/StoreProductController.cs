using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Abstract.Repositories;

namespace ASPAPI.Controllers {
    public record StoreProductDTO (int StoreID, int ProductId, int OrderItemId);
    [Route("[controller]/[action]")]
    [ApiController]
    public class StoreProductController : ControllerBase {
        public StoreProductController() {

        }
    }
}
