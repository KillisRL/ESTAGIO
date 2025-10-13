using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersDomain.Entidades;
using UsersInfraestrutura;
namespace BarbeariaMatutosAPI.Controllers
{

    [Route("api/Agendamentos")]
    [ApiController]
    public class FinalizarAgendamentoController: ControllerBase

    {
        private UserDBContext _db;
        public FinalizarAgendamentoController(UserDBContext dBContext)
        {
            this._db = dBContext;
        }
        [HttpGet ("barbeiros")]
        public async Task<IActionResult> Get() 
        {
            // Usando a versão assíncrona para não bloquear o servidor
            var barbeiros = await _db.Barbeiros.ToListAsync();
            return Ok(barbeiros);
        }

        // MÉTODO POST PROFISSIONAL
        [HttpPost]
        public async Task<IActionResult> CriarAgendamento(CriarAgendamentoDTO request)
        {
            if (request == null)
            {
                return BadRequest("Dados do agendamento inválidos.");
            }
            var novoAgendamento = new Agendamento
            {
                IdBarbeiro = request.IdBarbeiro,
                IdServico = request.IdServico,
                DataHora = request.DataHora,
                IdSituacao = request.IdSituacao,
                IDUsuario = request.IDUsuario
            };

            _db.Agendamentos.Add(novoAgendamento);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAgendamentoPorId), new { id = novoAgendamento.IdAgendamento }, novoAgendamento);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgendamentoPorId(int id)
        {
            var agendamento = await _db.Agendamentos
                                 .Include(a => a.Barbeiro.IdBarbeiro)
                                 .Include(a => a.Servico.IdServico)
                                 .FirstOrDefaultAsync(a => a.IdAgendamento == id);

            if (agendamento == null)
            {
                return NotFound(); // Retorna 404 se não encontrar
            }

            return Ok(agendamento); // Retorna 200 com o agendamento encontrado
        }
    }
}
