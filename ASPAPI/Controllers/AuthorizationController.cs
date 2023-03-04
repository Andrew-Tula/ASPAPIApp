using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ASPAPI.Controllers {
    public record RegistrationDto(string name, string password, int roleId);
    public record LoginDto(string name, string password);
    public record RefreshTokenVM(string token, string refreshToken);
    public record TokenVM(string token, DateTime tokenExpiration, string refreshToken);

    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase {
        private const int TOKEN_EXPIRE_MINUTES = 60;

        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUserTokeRepository userTokenRepository;

        public AuthorizationController(
            IConfiguration configuration, 
            IUserRepository userRepository, 
            IRoleRepository roleRepository, 
            IUserTokeRepository userTokeRepository) {
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userTokenRepository = userTokeRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Registration(RegistrationDto data) {
            if (string.IsNullOrWhiteSpace(data?.name) || string.IsNullOrWhiteSpace(data?.password))
                return BadRequest("Необходимо указать логин и пароль");

            string name = data.name;
            if (userRepository.GetByName(name) is not null)
                return BadRequest("Пользователь с таким названием уже зарегистрирован");

            int roleId = data.roleId;
            if (roleRepository.GetById(roleId) is null)
                return BadRequest("Роль не найдена");

            var salt = AccountHelper.CreateSaltKey();
            var hash = AccountHelper.CreatePasswordHash(data.password, salt);

            var user = new User {
                Name = name,
                Salt = salt,
                Hash = hash,
                RoleId = roleId
            };

            userRepository.Add(user);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginDto data) {
            if (string.IsNullOrWhiteSpace(data?.name) || string.IsNullOrWhiteSpace(data?.password))
                return BadRequest("Необходимо указать логин и пароль");

            var user = userRepository.GetByName(data.name);
            if (user is null)
                return BadRequest("Пользователь не найден");

            var hash = AccountHelper.CreatePasswordHash(data.password, user.Salt);
            if (hash != user.Hash)
                return BadRequest("Неверный пароль");

            var result = BuildToken(user.Id, data.name);
            return Ok(result);
        }

        private TokenVM BuildToken(int userId, string login) {
            var expireDate = DateTime.Now.AddMinutes(TOKEN_EXPIRE_MINUTES);
            string token = GenerateToken(userId, login, expireDate);
            string refreshToken = GenerateRefreshToken();
            var userToken = userTokenRepository.GetByUser(userId);
            if (userToken is null) {
                userToken = new UserToken {
                    UserId = userId,
                    RefreshToken = refreshToken,
                    CreationDate = DateTime.Now,
                };
                userTokenRepository.Add(userToken);
            } else {
                userToken.RefreshToken = refreshToken;
                userToken.CreationDate = DateTime.Now;
                userTokenRepository.Update(userToken);
            }
            
            return new TokenVM(token, expireDate, refreshToken);
        }

        private string GenerateToken(int userId, string login, DateTime expireDate) {
            var claims = new[] {
                new Claim("Id", userId.ToString()),
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Issuer"], claims,
                expires: expireDate, signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string GenerateRefreshToken() {
            using var randomGenerator = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            randomGenerator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        [HttpPost(nameof(RefreshToken))]
        public IActionResult RefreshToken(RefreshTokenVM data) {
            var tokenInfo = userTokenRepository.FindByToken(data.refreshToken);
            if (tokenInfo is null)
                return BadRequest("Данные не найден");

            var user = userRepository.GetById(tokenInfo.UserId);
            if (user is null)
                return BadRequest("Пользователь не найден");

            var expireDate = DateTime.Now.AddMinutes(TOKEN_EXPIRE_MINUTES);
            string token = GenerateToken(user.Id, user.Name, expireDate);

            var result = new TokenVM(token, expireDate, data.refreshToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult TestAuthorization() {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (userId is null)
                return BadRequest("Пользователь не найден");

            var user = userRepository.GetById(Convert.ToInt32(userId));
            if (user is null)
                return BadRequest();

            return Ok("Авторизван");
        }
    }
}
