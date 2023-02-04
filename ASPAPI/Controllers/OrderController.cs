using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;
using ASPAPI.Abstract.Repositories;

namespace ASPAPI.Controllers
{
    public record OrderDto(int id, string name, DateTime dtAndTime, int userid);
    [Route("[controller]/[action]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private IGenericRepositories<Order> orderRepository;
        public OrderController(IGenericRepositories<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public IActionResult GetOrder()
        {
            return Ok(orderRepository.GetAll());
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
       //         User = userRepository.GetUser(data.userid)
            };
            orderRepository.Add(order);        
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteOrder(int id)
        {
            var orderToDelete = orderRepository.GetById(id);  
            if (orderToDelete == null)
                return NotFound("Такого заказа не существует");
            orderRepository.Remove(orderToDelete);
            return Ok();
        }

        [HttpPut]
        public IActionResult EditOrder(OrderDto data)
        {
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните название / никнейм заказа");

            // var orderToUpdate = orderRepository.GetById(data.id); 
            var orderToUpdate = orderRepository.GetById(data.id);

            if (orderToUpdate == null)
                return NotFound("Такого заказа не существует");

            orderToUpdate.Name=data.name;

            orderRepository.Update(orderToUpdate);
            return Ok();
        }





    }



}