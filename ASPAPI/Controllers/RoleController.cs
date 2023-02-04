using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Abstract.Repositories;

namespace ASPAPI.Controllers {
    public record RoleDto(int id, string name);

    [Route("[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase {
        private IRoleRepository roleRepository;

        public RoleController(IRoleRepository roleRepository) 
        {
            this.roleRepository = roleRepository;
        }

        [HttpGet]
        public IActionResult GetRoles() => Ok(roleRepository.GetAll());

        [HttpPost]
        public IActionResult AddRole(string name) {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Заполните данные");

            var searchedRole = roleRepository.GetByName(name);
            if (searchedRole != null)
                return BadRequest("Данная роль уже присутствует");

            var role = new Role {
                Name = name
            };
            roleRepository.Add(role);
            return Ok();
        }

        [HttpPut] // написать метод change
        public IActionResult EditRole(RoleDto role)
        {
            if (string.IsNullOrWhiteSpace(role?.name))
                return BadRequest("Не задана изменяемая роль/имя");

            var roleToUpdate = roleRepository.GetById(role.id);
            if (roleToUpdate == null) 
                return NotFound("Такой роли не существует");

            roleToUpdate.Name = role.name;
            roleRepository.Update(roleToUpdate);
            return Ok();
        }


        [HttpDelete] // написать метод delete
        public IActionResult DeleteRole(int id) {
            var roleToDelete = roleRepository.GetById(id);
            if (roleToDelete == null) 
                return NotFound("Такой роли не существует");

            roleRepository.Remove(roleToDelete);
            return Ok();
        }
    }
}
