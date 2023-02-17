using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Abstract.Repositories;
using ASPAPI.Repositories;

namespace ASPAPI.Controllers 
{
    public record StoreProductDTO (int StoreId, int ProductId, int StoreCount);
    [Route("[controller]/[action]")]
    [ApiController]
    public class StoreProductController : ControllerBase 
    {
        private IStoreProductRepository storeProductRepository;
       // private IGenericRepositories<StoreProduct> storeProductRepository;

        private IOrderItemRepository orderItemRepository;
       // private IGenericRepositories<OrderItem> orderItemRepository;

        private IProductRepository productRepository;

        private IStoreRepository storeRepository;

        public StoreProductController(IStoreProductRepository storeProductRepository) //
            //  GenericRepositories<StoreProduct> productRepository)
        {
            this.storeProductRepository = storeProductRepository;
        }

        [HttpGet]
        public IActionResult GetStoreProduct()
        {
            return Ok(storeProductRepository.GetAll());
        }

        [HttpPost]
        public IActionResult AddStoreProduct(StoreProductDTO data)
        {
            // есть ли метод, проверящий корректность ввода ЧИСЛА? (TryParse?)
            if (data.StoreId == ' ' || data.StoreId < '0' || data.StoreId > '9')
                return BadRequest("Заполните номер (ID) магазина");
            if (data.ProductId == ' ' || data.ProductId < '0')
                return BadRequest("Заполните ID продукта");
            if (data.StoreCount == ' ' || data.StoreCount == 0)
                return BadRequest("Заполните количество товара / продукта");

            var storeProduct = new StoreProduct
            {
                StoreCount = data.StoreCount,
                StoreId = data.StoreId,
                ProductId = data.ProductId,
                //         User = userRepository.GetUser(data.userid)
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
            if (data.StoreId == ' ' || data.StoreId < '0' || data.StoreId > '9')
                return BadRequest("Заполните номер (ID) магазина");
            if (data.ProductId == ' ' || data.ProductId < '0')
                return BadRequest("Заполните ID продукта");
            if (data.StoreCount == ' ' || data.StoreCount <= 0)
                return BadRequest("Заполните количество товара / продукта");

            var originStoreProduct = storeProductRepository.GetById(data.StoreCount);
            if (originStoreProduct is null)
                return BadRequest("Элемент заказа не найден");
            if (data.StoreCount == originStoreProduct.StoreCount)
                return Ok();

            var storeProduct = new StoreProduct
            {
                StoreCount = data.StoreCount,
                StoreId = data.StoreId,
                ProductId = data.ProductId,
                //         User = userRepository.GetUser(data.userid)
            };
            storeProductRepository.Update(storeProduct);
            return Ok();
        }
    }
    
}
