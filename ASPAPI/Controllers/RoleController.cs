using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASPAPI.Controllers {
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
        public IActionResult EditRole(string name, string newName)
        {
            if (name is null || string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Не задана изменяемая роль/имя");
            }

            if (newName is null || string.IsNullOrWhiteSpace(newName)) 
            {
                return BadRequest("Не задана новая роль/имя");
            }
         var roleToUpdate = dbContext.Roles.FirstOrDefault(r => Role.name == name);

            if (roleToUpdate == null) return NotFound("Такой роли не существует");

            roleToUpdate.Name = newName;
            dbContext.Roles.Update(roleToUpdate);
            dbContext.SaveChanges();
            return Ok();
        }
        
                  
         [HttpDelete] // написать метод delete
         public IActionResult DeleteRole(string name)
        {
            if (name is null || string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Не задана изменяемая роль/имя");
            }
            var roleToDelete = dbContext.Roles.FirstOrDefault(r => Role.name == name);
            if (roleToDelete == null) return NotFound("Такой роли не существует");
            roleToDelete.Name = name;
            dbContext.Roles.Remove(roleToDelete);   
            dbContext.SaveChanges();
            return Ok();

        }

    }
}
