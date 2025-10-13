using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient; // Ou Microsoft.Data.SqlClient
using System.Collections.Generic;
using UsersInfraestrutura;
using System.Runtime.CompilerServices;
using UsersDomain.Entidades;

namespace BarbeariaMatutosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        private UserDBContext _db;

        public ServicosController(UserDBContext dBContext)
        {
            this._db = dBContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var servicos = _db.Services.ToList();
            return Ok(servicos);
        }
    }
}
