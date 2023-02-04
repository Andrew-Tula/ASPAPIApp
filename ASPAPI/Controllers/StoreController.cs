using System;
using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;
using ASPAPI.Abstract.Repositories;
using ASPAPI.Repositories;

namespace ASPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public record ProductDeleteDto(int id);
    public record ProductPostDto(int id, string name, int storeCount);
    public class StoreController : ControllerBase
    {
        private IStoreRepository storeRepository;
        public StoreController(IStoreRepository storeRepository) => this.storeRepository = storeRepository;

        [HttpGet]
        public IActionResult GetStores() => Ok(storeRepository.GetAll());

        [HttpDelete]
        public IActionResult DeleteProduct(ProductDeleteDto data)
        {
            //var productToDelete = storeRepository.GetById(data);
            //if (productToDelete == null)
            //    return NotFound("Такого подукта не существует");

            //storeRepository.Remove(productToDelete);
            //не выполнится. надо как-то обратиться с удалением не к своей таблице (каскадно удалить?)  
             return Ok();
        }

        [HttpPost]
        public IActionResult AddStoreCountForProduct(ProductPostDto data)
        {
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните название продукта");
            if (data.storeCount == 0 || data.storeCount == ' ')
                return BadRequest("Заполните количество продукта");

            //var storeProduct = new Store
            //{ StoreCount = data.storeCount };

            //storeRepository.Add(storeProduct);
            return Ok();
        }
    }
}
