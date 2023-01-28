using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Controllers
{
    public record OrderDto(string name, DateTime dtAndTime, int userid);
    [Route("[controller]/[action]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private TestDBContext dbContext;
        public OrderController(TestDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetOrder()
        {
            return Ok(dbContext.Orders?.ToList());
        }

        [HttpPost]
        public IActionResult AddOrder(OrderDto data)
        {
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните название / никнейм заказа");
            if (data.userid == 0 || data.userid == ' ')
                return BadRequest("Не передан ID пользователя");

            var order = new Order
            { Name = data.name, Date = data.dtAndTime, UserId = data.userid, 
                User = dbContext.Users.FirstOrDefault(u => u.Id == data.userid) };
        //  Console.WriteLine(order);
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteOrder(int id)
        {
            var OrderToDelete = dbContext.Orders.FirstOrDefault(o => o.Id == id);
            if (OrderToDelete == null)
                return NotFound("Такого заказа не существует");

            dbContext.Orders.Remove(OrderToDelete);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult EditOrder(OrderDto data)
        {
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните название / никнейм заказа");

            var OrderToUpdate = dbContext.Orders.FirstOrDefault(o => o.Id == data.userid);
            if (OrderToUpdate == null)
                return NotFound("Такого заказа не существует");

          //  OrderToUpdate.Name = data.name;
            dbContext.Orders.Update(OrderToUpdate);
            dbContext.SaveChanges();
            return Ok();
        }





    }



}