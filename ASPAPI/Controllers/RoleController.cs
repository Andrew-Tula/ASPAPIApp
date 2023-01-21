using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Controllers {
    public record RoleDto(int id, string name);

    [Route("[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase {
        private TestDBContext dbContext;
        public RoleController(TestDBContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetRoles() {
            return Ok(dbContext.Roles?.ToList());
        }

        [HttpPost]
        public IActionResult AddRole(string name) {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Заполните данные");

            var role = new Role {
                Name = name,
            };
            dbContext.Roles.Add(role);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut] // написать метод change
        public IActionResult EditRole(RoleDto role)
        {
            if (string.IsNullOrWhiteSpace(role?.name))
                return BadRequest("Не задана изменяемая роль/имя");

            var roleToUpdate = dbContext.Roles.FirstOrDefault(r => r.Id == role.id);
            if (roleToUpdate == null) 
                return NotFound("Такой роли не существует");

            roleToUpdate.Name = role.name;
            dbContext.Roles.Update(roleToUpdate);
            dbContext.SaveChanges();
            return Ok();
        }


        [HttpDelete] // написать метод delete
        public IActionResult DeleteRole(int id) {
            var roleToDelete = dbContext.Roles.FirstOrDefault(r => r.Id == id);
            if (roleToDelete == null) 
                return NotFound("Такой роли не существует");

            dbContext.Roles.Remove(roleToDelete);
            dbContext.SaveChanges();
            return Ok();

        }
    }
}
