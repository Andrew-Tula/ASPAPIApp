using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using Microsoft.AspNetCore.Mvc;

namespace ASPAPI.Controllers {
    public record StoreDto(string name, string address);

    [Route("[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IGenericRepository<Store> storeRepository;

        public StoreController(IGenericRepository<Store> storeRepository) => this.storeRepository = storeRepository;

        [HttpGet]
        public IActionResult GetStores() => Ok(storeRepository.GetAll());

        [HttpPost]
        public IActionResult AddStore(StoreDto data) {
          
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Укажите название");

            if (string.IsNullOrWhiteSpace(data.address))
                return BadRequest("Укажите адрес");

            if (data is null)
                return BadRequest("Данные пусты");

            var store = new Store {
                Name = data.name,
                Address = data.address
            };
            storeRepository.Add(store);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteStore(int id) {
            var store = storeRepository.GetById(id);
            if (store is null)
                return BadRequest("Магазин не найден");

            storeRepository.Remove(store);
            return Ok();
        }

        [HttpPut]
        public IActionResult EditStore(Store store) {
            var originStore = storeRepository.GetById(store.Id);
            if (originStore is null)
                return BadRequest("Магазин не найден");

            originStore.Name = store.Name;
            originStore.Address = store.Address;
            storeRepository.Update(originStore);
            return Ok();
        }
    }
}
