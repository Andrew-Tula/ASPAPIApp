using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Abstract.Repositories;

namespace ASPAPI.Controllers {
    public record StoreProductDTO (int StoreId, int ProductId, int StoreCount);
    [Route("[controller]/[action]")]
    [ApiController]
    public class StoreProductController : ControllerBase 
    {
        private IStoreProductRepository storeProductRepository;
       // private IGenericRepositories<StoreProduct> storeProductRepository;

        //private IOrderItemRepository orderItemRepository;
       // private IGenericRepositories<OrderItem> orderItemRepository;

        private IGenericRepository<Product> productRepository;
        private IGenericRepository<Store> storeRepository;

        public StoreProductController(
            IStoreProductRepository storeProductRepository,
            IGenericRepository<Store> storeRepository,
            IGenericRepository<Product> productRepository)
        {
            this.storeProductRepository = storeProductRepository;
            this.storeRepository = storeRepository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetStoreProduct()
        {
            return Ok(storeProductRepository.GetAll());
        }

        [HttpPost]
        public IActionResult AddStoreProduct(StoreProductDTO data)
        {
            var store = storeRepository.GetById(data.StoreId);
            if (store is null)
                return BadRequest("Нет магазина");

            var product = productRepository.GetById(data.ProductId);
            if (product is null)
                return BadRequest("Нет продукта");

            if (data.StoreCount == 0)
                return BadRequest("Заполните количество товара / продукта");

            var storeProduct = new StoreProduct
            {
                StoreCount = data.StoreCount,
                StoreId = data.StoreId,
                ProductId = data.ProductId,
            };
            storeProductRepository.Add(storeProduct);
            return Ok();
        }
        //[HttpDelete]                       ВИДИМО, МЕТОД удаления НЕ ИМЕЕТ СМЫСЛА ?
        //public IActionResult DeleteStorecount(int id)
        //{
        //    var StoreProductToDelete = StoreProductRepository.GetById(id);
        //    if (StoreProductToDelete == null)
        //        return NotFound("Такого продукта не существует");
        //    StoreProductRepository.Remove(StoreProductToDelete);
        //    return Ok();
        //}

        [HttpPut]
        public IActionResult EditStoreProduct(StoreProductDTO data)
        {
            var store = storeRepository.GetById(data.StoreId);
            if (store is null)
                return BadRequest("Нет магазина");

            var product = productRepository.GetById(data.ProductId);
            if (product is null)
                return BadRequest("Нет продукта");

            if (data.StoreCount <= 0)
                return BadRequest("Заполните количество товара / продукта");

            var originStoreProduct = storeProductRepository.GetById(data.ProductId);
            if (originStoreProduct is null)
                return BadRequest("Элемент заказа не найден");

            if (data.StoreCount == originStoreProduct.StoreCount)
                return Ok();

            originStoreProduct.StoreCount = data.StoreCount;
            originStoreProduct.StoreId = data.StoreId;
            originStoreProduct.ProductId = data.ProductId;
            storeProductRepository.Update(originStoreProduct);
            return Ok();
        }
    }
    
}
