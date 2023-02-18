using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;
using ASPAPI.Abstract.Repositories;

namespace ASPAPI.Controllers
{
    public record OrderDto(int id, string name);
    public record OrderBaseDto(string name, int userid);

    [Route("[controller]/[action]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;

        public OrderController(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetOrder()
        {
            return Ok(orderRepository.GetAll());
        }

        [HttpPost]
        public IActionResult AddOrder(OrderBaseDto data)
        {
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните название / никнейм заказа");

            var user = userRepository.GetById(data.userid);
            if (user is null)
                return BadRequest("Пользовательне найден");

            var order = new Order
            { 
                Name = data.name, 
                Date = DateTime.Now, 
                UserId = data.userid,
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

            var orderToUpdate = orderRepository.GetById(data.id);
            if (orderToUpdate == null)
                return NotFound("Такого заказа не существует");

            orderToUpdate.Name=data.name;
            orderRepository.Update(orderToUpdate);
            return Ok();
        }
    }
}