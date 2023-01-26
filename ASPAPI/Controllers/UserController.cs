using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Controllers {
    public record UserDto(string name, int roleId);
    public record UserExtendedDto(int id, string name, int roleId): UserDto(name, roleId);

    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase {
        private TestDBContext dbContext;

        public UserController(TestDBContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetUsers() {
            return Ok(dbContext.Users?.ToList());
        }

        [HttpPost]
        public IActionResult AddUser(UserDto data) {
            if (data is null || string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните данные");

            var role = dbContext.Roles.FirstOrDefault(r => r.Id == data.roleId);
            if (role is null)
                return NotFound("Роль не существует");

            var user = new User { 
                Name = data.name,
                RoleId = role.Id,
            };
          //  Console.WriteLine(user);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id) {
            //var userToDelete = null;
            //foreach (var item in dbContext.Users) {
            //    if (item.Id == id) {
            //        userToDelete = item;
            //        break;
            //    }
            //}

            var userToDelete = dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (userToDelete is null)
                return NotFound("Такого пользователя не существует");

            dbContext.Users.Remove(userToDelete);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult ChangeUser(UserExtendedDto data) {
            if (data is null || string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Вы не ввели Имя пользователя для изменения");

            var userToUpdate = dbContext.Users.FirstOrDefault(u => u.Id == data.id);
            if (userToUpdate is null)
                return NotFound("Такого пользователя не существует");

            userToUpdate.Name = data.name;
            userToUpdate.RoleId = data.roleId;
            dbContext.Users.Update(userToUpdate);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
