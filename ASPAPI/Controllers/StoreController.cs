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
    public record ProductDto(int id, int storeCount);
    public record ProductPostDto(int id, string name, int storeCount);
    public class StoreController : ControllerBase
    {
        private IStoreRepository storeRepository;
        public StoreController(IStoreRepository storeRepository) => this.storeRepository = storeRepository;

        [HttpGet]
        public IActionResult GetStores() => Ok(storeRepository.GetAll());

        [HttpDelete]
        // На самом деле в Сторе лежит внешний ключ на Продукт, поэтому операция "удаление" обнуляет количество 
        public IActionResult DeleteProduct(ProductDto data)
        {
            var productToDelete = storeRepository.GetById(data.id);
            if (productToDelete == null)
                return NotFound("Такого подукта не существует");
            productToDelete.StoreCount = 0;
    //  ???  верно ли такое действие (обнуление остатков) и верен ли синтаксис ? 

            storeRepository.Update(productToDelete);
            
             return Ok();
        }

        [HttpPut]
        public IActionResult ChangeStoreCount(ProductDto data)
        {
            var countToUpdate = storeRepository.GetById(data.id);
            if (countToUpdate == null)
                return NotFound("Такого подукта не существует");
            if (data.storeCount <= 0) 
                return BadRequest("Количество единиц товара не может быть нулевым или отрицательным");
            
            countToUpdate.StoreCount = data.storeCount;
           
            storeRepository.Update(countToUpdate);
            return Ok();
        }

        [HttpPost]
        public IActionResult AddStoreCountForProduct(ProductPostDto data)
        {
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните название продукта");
            if (data.storeCount == 0 || data.storeCount == ' ')
                return BadRequest("Заполните количество продукта");

            var storeProduct = new Store
            { StoreCount = data.storeCount };

            storeRepository.Add(storeProduct);
            return Ok();
        }
    }
}
