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
        [HttpGet("consultar")]
        public IActionResult Get()
        {
            var servicos = _db.Servicos.ToList();
            return Ok(servicos);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CreateServico(Servicos servicos)
        {
            var Criarservicos = new Servicos
            {
                IdServico = servicos.IdServico,
                DescServico = servicos.DescServico,
                Duracao = servicos.Duracao,
                ValorServico = servicos.ValorServico,
                TempoEstimadoMinutos = servicos.TempoEstimadoMinutos
            };

            _db.Servicos.Add(Criarservicos);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
