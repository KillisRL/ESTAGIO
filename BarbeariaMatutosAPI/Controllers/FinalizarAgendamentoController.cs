using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using UsersDomain.Entidades;
using UsersInfraestrutura;
using static UsersDomain.Entidades.SituacaoAgendamentoEnum;
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
            return CreatedAtAction(nameof(ConsultarAgendamento), new { id = novoAgendamento.IdAgendamento }, novoAgendamento);
        }

        [HttpGet("consulta")]
        public async Task<IActionResult> ConsultarAgendamento()
        {
            try { 
            var agendamento = await _db.Agendamentos
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .Include(a => a.Users)
                .Include(a => a.AgendamentoSituacao)

                .Select(a => new
                {
                    a.IdAgendamento,
                    a.IdServico,
                    DescServico = a.Servico.DescServico,
                    a.DataHora,
                    a.IdBarbeiro,
                    NomeBarbeiro = a.Barbeiro.NomeBarbeiro,
                    a.IDUsuario,
                    NomeUsuario = a.Users.Nome,
                    Email = a.Users.Email,
                    IdSituacao = a.IdSituacao,
                    DescSituacao = a.AgendamentoSituacao.DescSituacao
                }).ToListAsync();
                return Ok(agendamento);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Erro interno ao buscar agendamentos: {ex.Message}");
            }
        }
        [HttpGet("consulta/{id:int}")]
        public async Task<IActionResult> GetAgendamentoPorId(int id)
        {
            var agendamento = await _db.Agendamentos
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .Include(a => a.Users)
                .Include(a => a.AgendamentoSituacao)
                .Select(a => new
                {
                    a.IdAgendamento,
                    a.IdServico,
                    DescServico = a.Servico.DescServico,
                    a.DataHora,
                    a.IdBarbeiro,
                    NomeBarbeiro = a.Barbeiro.NomeBarbeiro,
                    a.IDUsuario,
                    NomeUsuario = a.Users.Nome,
                    Email = a.Users.Email,
                    IdSituacao = a.IdSituacao,
                    DescSituacao = a.AgendamentoSituacao.DescSituacao
                })
                .FirstOrDefaultAsync(a => a.IdAgendamento == id); // <--- AQUI ESTÁ O WHERE

            if (agendamento == null)
            {
                return NotFound($"Agendamento com ID {id} não encontrado.");
            }

            return Ok(agendamento);
        }

        [HttpGet("meus-agendamentos/{userId:int}")] // <-- REMOVA o "/{userId:int}" daqui
        public async Task<IActionResult> ConsultarMeusAgendamentos(int userId) // <-- Método continua sem parâmetros
        {
            try
            {

                // A consulta usa o IDUsuario obtido das Claims
                var agendamentos = await _db.Agendamentos
                    .Include(a => a.Barbeiro)
                    .Include(a => a.Servico)
                    .Include(a => a.Users)
                    .Include(a => a.AgendamentoSituacao)
                    .Where(a => a.IDUsuario == userId) // Filtro pelo usuário logado
                    .Select(a => new
                    {
                        a.IdAgendamento,
                        a.IdServico,
                        DescServico = a.Servico.DescServico,
                        a.DataHora,
                        a.IdBarbeiro,
                        NomeBarbeiro = a.Barbeiro.NomeBarbeiro,
                        a.IDUsuario,
                        NomeUsuario = a.Users.Nome,
                        Email = a.Users.Email,
                        IdSituacao = a.IdSituacao,
                        DescSituacao = a.AgendamentoSituacao.DescSituacao
                    })
                    .OrderByDescending(a => a.DataHora)
                    .ToListAsync();

                return Ok(agendamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao buscar seus agendamentos: {ex.Message}");
            }
        }
        [HttpPatch("AlterarStatus/{agendamentoId:int}")]
        public async Task<IActionResult> AlterarStatusAgendamento(
             int agendamentoId,
             [FromBody] AlterarStatusRequestDto request) // 1. Recebe o DTO do corpo
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var agendamento = await _db.Agendamentos
                                           .FirstOrDefaultAsync(a => a.IdAgendamento == agendamentoId);

                if (agendamento == null)
                {
                    return NotFound($"Agendamento com ID {agendamentoId} não encontrado.");
                }
                agendamento.IdSituacao = request.NovoStatusId;
                // ---------------------------

                await _db.SaveChangesAsync();

                return NoContent(); // Sucesso (204)
            }
            catch (Exception ex)
            {
                // Mensagem de erro atualizada
                return StatusCode(500, $"Erro interno ao alterar o status do agendamento: {ex.Message}");
            }
        }
        //http para que o barbeiro logado possa visualizar seus atendimentos
        [HttpGet("meus-servicos/{IdBarbeiro:int}")] // <-- REMOVA o "/{userId:int}" daqui
        public async Task<IActionResult> ConsultarMeusServicos(int IdBarbeiro) // <-- Método continua sem parâmetros
        {
            try
            {


                // A consulta usa o IDUsuario obtido das Claims
                var agendamentos = await _db.Agendamentos
                    .Include(a => a.Barbeiro)
                    .Include(a => a.Servico)
                    .Include(a => a.Users)
                    .Include(a => a.AgendamentoSituacao)
                    .Where(a => a.IdBarbeiro == IdBarbeiro) // Filtro pelo usuário logado
                    .Select(a => new
                    {
                        a.IdAgendamento,
                        a.IdServico,
                        DescServico = a.Servico.DescServico,
                        a.DataHora,
                        a.IdBarbeiro,
                        NomeBarbeiro = a.Barbeiro.NomeBarbeiro,
                        a.IDUsuario,
                        NomeUsuario = a.Users.Nome,
                        Email = a.Users.Email,
                        IdSituacao = a.IdSituacao,
                        DescSituacao = a.AgendamentoSituacao.DescSituacao
                    })
                    .OrderByDescending(a => a.DataHora)
                    .ToListAsync();

                return Ok(agendamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao buscar seus agendamentos: {ex.Message}");
            }
        }
    }
}
