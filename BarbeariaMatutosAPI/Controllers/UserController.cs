using Microsoft.AspNetCore.Mvc;
using UsersInfraestrutura;
using UsersDomain;
using UsersDomain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BarbeariaMatutosAPI.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private UserDBContext _db;
        public UserController(UserDBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet("Consulta")]
        public IActionResult Get()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }
        /*[HttpPost]
        public IActionResult Add(Users user)
        {
            var users = _db.Users.Add(user);
            _db.SaveChanges();

            return Ok(users.Entity);
        }
        */
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> CreateUser(UserCreate userCreate)
        {
            var user = new Users
            {
                Nome = userCreate.Nome,
                Email = userCreate.Email,
                Telefone = userCreate.Telefone,
                SenhaHash = userCreate.SenhaHash
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(userCreate.Entity);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            // Procura o usuário no banco
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.SenhaHash == login.SenhaHash);

            if (user == null)
            {
                return Unauthorized(new { Message = "E-mail ou senha inválidos!" });
            }

            return Ok(user);
        }
    }
}
