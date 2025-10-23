
using System.Diagnostics;
using System.Net.Http.Json;
using UsersDomain.Entidades;

namespace BarbeariaMatutosApp.Services
{
    public class ApiServices
    {
        private readonly HttpClient _httpClient;

        private const string ApiBaseURL = "http://localhost:5125"; // Ou pegue de um arquivo de config// Ou pegue de um arquivo de config

        public ApiServices()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(ApiBaseURL) };
        }

        public async Task<List<Servicos>> GetServicosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Servicos>>("api/Servicos");
        }

        public async Task<List<Barbeiro>> GetBarbeirosAsync()
        {
            // Supondo que você tenha um endpoint para barbeiros
            return await _httpClient.GetFromJsonAsync<List<Barbeiro>>("api/Agendamentos/Barbeiros");
        }
        public async Task<List<AgendamentoDTO>> GetAgendamentoAsync()
        {
            try
            {
                // Agora esperamos uma lista do tipo AgendamentoDto
                var agendamentos = await _httpClient.GetFromJsonAsync<List<AgendamentoDTO>>("api/Agendamentos/consulta");
                return agendamentos ?? new List<AgendamentoDTO>();
            }
            catch (Exception ex)
            {
                // Adicione um log para depuração
                Debug.WriteLine($"Erro ao buscar agendamentos: {ex.Message}");
                return new List<AgendamentoDTO>(); // Retorna uma lista vazia em caso de erro
            }
        }
        public async Task<List<AgendamentoDTO>> GetMeusAgendamentosAsync(int userId) // Recebe o ID
        {
            try
            {
                string apiUrl = $"api/Agendamentos/meus-agendamentos/{userId}"; // Usa o ID na URL
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Verifica se a API retornou sucesso (2xx)
                var agendamentos = await response.Content.ReadFromJsonAsync<List<AgendamentoDTO>>();
                return agendamentos ?? new List<AgendamentoDTO>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao buscar 'meus' agendamentos para usuário {userId}: {ex.Message}");
                return new List<AgendamentoDTO>();
            }
        }
        public async Task<bool> SalvarAgendamentoAsync(CriarAgendamentoDTO agendamentoRequest)
        {
            try
            {
                // Usa PostAsJsonAsync para serializar o objeto para JSON e enviá-lo no corpo da requisição POST
                var response = await _httpClient.PostAsJsonAsync("api/Agendamentos", agendamentoRequest);

                // Retorna true se a resposta da API foi bem-sucedida (código de status 2xx)
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log do erro para depuração
                Debug.WriteLine($"Erro ao salvar agendamento: {ex.Message}");
                return false;
            }
        }
        public async Task<(Users usuario, string erro)> Login(string email, string senha)
        {
            // 1. Mudamos o retorno para <(Usuario usuario, string erro)>
            try
            {
                var loginRequest = new { SenhaHash = senha, Email = email };
                var response = await _httpClient.PostAsJsonAsync("User/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    // 2. Lemos o corpo da resposta e transformamos em um objeto Usuario
                    Users usuarioLogado = await response.Content.ReadFromJsonAsync<Users>();

                    // 3. Retornamos o objeto do usuário e um erro nulo
                    return (usuarioLogado, null);
                }
                else
                {
                    var erro = await response.Content.ReadAsStringAsync();

                    // 4. Retornamos um usuário nulo e a mensagem de erro
                    return (null, erro);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro de conexão no login: {ex.Message}");
                // 5. Em caso de exceção, também retornamos um usuário nulo
                return (null, $"Erro de conexão: {ex.Message}");
            }
        }
    }
}
