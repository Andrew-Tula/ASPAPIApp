using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Abstract.Repositories;

namespace ASPAPI.Controllers {
    public record OrderItemDto(int productCount, int storeProductId, int orderid);
    public record OrderItemChangeDto(int orderItemId, int productCount);

    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private IOrderItemRepository orderItemRepository;
        private IStoreProductRepository storeProductRepository;

        public OrderItemController(IOrderItemRepository orderItemRepository, IStoreProductRepository productRepository)
        {
            this.orderItemRepository = orderItemRepository;
            this.storeProductRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetOrderItem() => Ok(orderItemRepository.GetAll());
      
        [HttpPost]
        public IActionResult AddOrderItem(OrderItemDto data)
        {
            if (data is null)
                return BadRequest("Пустое поле данных");

            if (data.productCount <= 0)
                return BadRequest("Количество товара не может быть нулевым или отрицательным");

            var storeProduct = storeProductRepository.GetById(data.storeProductId);
            if (storeProduct is null)
                return BadRequest("Продукт не существует");

            if (storeProduct.StoreCount < data.productCount)
                return BadRequest("Нет достаточного количества продукта в магазине");

            var order = orderItemRepository.GetOrder(data.orderid);
            if (order is null)
                return BadRequest("Заказ не существует");

            var orderItem = new OrderItem
            {
                ProductCount = data.productCount,
                StoreProductId = data.storeProductId,
                OrderId = data.orderid,
            };

            orderItemRepository.Add(orderItem);

            storeProduct.StoreCount -= data.productCount;
            storeProductRepository.Update(storeProduct);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteOrderItem(int id)
        {
            var orderItemToDelete = orderItemRepository.GetById(id);
            if (orderItemToDelete == null)
                return NotFound("Нет такого продукта");
            orderItemRepository.Remove(orderItemToDelete);
            return Ok();
        }

        [HttpPut]
        public IActionResult EditOrderItem(OrderItemChangeDto data)
        {
            if (data is null)
                return BadRequest("Не заполнены поля для изменения");

            if (data.productCount <= 0)
                return BadRequest("Количество товара не может быть нулевым или отрицательным");

            //var originOrderItem = dbContext.OrderItems.FirstOrDefault(o => o.Id == data.orderItemId);
            var originOrderItem = orderItemRepository.GetById(data.orderItemId);
            if (originOrderItem is null)
                return BadRequest("Элемент заказа не найден");

            if (data.productCount == originOrderItem.ProductCount)
                return Ok();

            originOrderItem.ProductCount = data.productCount;
            orderItemRepository.Update(originOrderItem);
            return Ok();
        }
    }
}