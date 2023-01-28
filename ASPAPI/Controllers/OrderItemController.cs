using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Controllers
{
    public record OrderItemDto(int productCount, int productid, int orderid);
    public record OrderItemChangeDto(int orderItemId, int productCount);

    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private const string DataIsEmpty = "Данные пусты";
        private TestDBContext dbContext;

        public OrderItemController(TestDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetOrderItem() => Ok(dbContext.OrderItems?.ToList());
        //{
        //    return Ok(dbContext.OrderItems?.ToList());
        //}

        [HttpPost]
        public IActionResult AddOrderItem(OrderItemDto data)
        {
            if (data is null)
                return BadRequest(DataIsEmpty);

            if (data.productCount <= 0)
                return BadRequest("Количество товара не может быть нулевым или отрицательным");

            var product = dbContext.Products.FirstOrDefault(p => p.Id == data.productid);
            if (product is null)
                return BadRequest("Продукт не существует");

            var order = dbContext.Orders.FirstOrDefault(o => o.Id == data.orderid);
            if (order is null)
                return BadRequest("Заказ не существует");

            var orderItem = new OrderItem
            { 
                ProductCount = data.productCount,
                ProductId = data.productid,
                OrderId = data.orderid,
            };
            dbContext.OrderItems.Add(orderItem);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteOrderItem(int id)
        {
            var orderItemToDelete = dbContext.OrderItems.FirstOrDefault(o => o.Id == id);
            if (orderItemToDelete == null)
                return NotFound("Нет такого продукта");

            dbContext.OrderItems.Remove(orderItemToDelete);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult EditOrderItem(OrderItemChangeDto data)
        {
            if (data is null)
                return BadRequest(DataIsEmpty);

            if (data.productCount <= 0)
                return BadRequest("Количество товара не может быть нулевым или отрицательным");

            var originOrderItem = dbContext.OrderItems.FirstOrDefault(o => o.Id == data.orderItemId);
            if (originOrderItem is null)
                return BadRequest("Элемент заказа не найден");

            if (data.productCount == originOrderItem.ProductCount)
                return Ok();

            originOrderItem.ProductCount = data.productCount;
            dbContext.OrderItems.Update(originOrderItem);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}