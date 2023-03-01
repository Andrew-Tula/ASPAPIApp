using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Abstract.Repositories;

namespace ASPAPI.Controllers {
    public record UserDto(string name, int roleId);
    public record UserExtendedDto(int id, string name, int roleId): UserDto(name, roleId);

    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase {
        private IUserRepository userRepository;

        public UserController(IUserRepository userRepository) => this.userRepository = userRepository;

        [HttpGet]
        public IActionResult GetUsers() => Ok(userRepository.GetAll());

        [HttpPost]
        public IActionResult AddUser(UserDto data) {
            if (data is null || string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните данные");

            var role = userRepository.GetRole(data.roleId);
            if (role is null)
                return NotFound("Роль не существует");

            var user = new User { 
                Name = data.name,
              //  RoleId = role.Id,
                RoleId = data.roleId
            };
            userRepository.Add(user);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id) {
            var userToDelete = userRepository.GetById(id);
            if (userToDelete is null)
                return NotFound("Такого пользователя не существует");

            userRepository.Remove(userToDelete);
            return Ok();
        }

        [HttpPut]
        public IActionResult ChangeUser(UserExtendedDto data) {
            if (data is null || string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Вы не ввели Имя пользователя для изменения");

            var userToUpdate = userRepository.GetById(data.id);
            if (userToUpdate is null)
                return NotFound("Такого пользователя не существует");

            var role = userRepository.GetRole(data.roleId);
            if (role is null)
                return NotFound("Роль не существует");

            userToUpdate.Name = data.name;
            userToUpdate.RoleId = data.roleId;
            userRepository.Update(userToUpdate);
            return Ok();
        }
    }
}
