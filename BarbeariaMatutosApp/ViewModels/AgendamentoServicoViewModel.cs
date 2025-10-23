using BarbeariaMatutosApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using UsersDomain.Entidades;
using BarbeariaMatutosApp.Services;
namespace BarbeariaMatutosApp.ViewModels
{
    public partial class AgendamentoServicoViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseURL = "http://localhost:5125";

        [ObservableProperty]
        private string? nome;

        [ObservableProperty]
        private int? idUsuario;

        [ObservableProperty]
        private ObservableCollection<Servicos> servicos;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private Servicos? servicoSelecionado;
        public AgendamentoServicoViewModel()
        {
            CarregarDadosUsuario();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseURL)
            };
            // Inicialize a coleção
            Servicos = new ObservableCollection<Servicos>();

        }
        private void CarregarDadosUsuario()
        {
            var usuario = SessaoUsuarioService.Usuariologado;
            if (usuario != null)
            {
                Nome = usuario.Nome;       // "Bem Vindo - " + Nome
                IdUsuario = usuario.IDUsuario; // Agora você tem o ID para usar no agendamento
            }
        }
        [RelayCommand]
        private async Task LoadServicosAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var servicosDaApi = await _httpClient.GetFromJsonAsync<List<Servicos>>("api/Servicos");

                if (servicosDaApi != null)
                {
                    // Limpa e adiciona os itens na coleção
                    Servicos.Clear();
                    foreach (var servico in servicosDaApi)
                    {
                        Servicos.Add(servico);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Não foi possível carregar os serviços. Verifique a conexão com a API. Erro: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task AgendarServicoAsync()
        {
            if (ServicoSelecionado != null)
            {
                // Aqui você pode usar as informações do serviço selecionado.
                int idDoServico = ServicoSelecionado.IdServico;
                string nomeDoServico = ServicoSelecionado.DescServico;

                await Application.Current.MainPage.DisplayAlert("Serviço Selecionado",
                    $"Você selecionou o serviço: {nomeDoServico} com ID: {idDoServico}", "OK");

                if (ServicoSelecionado != null)
                {
                    // O jeito NOVO e CORRETO (usando Shell e passando parâmetros):

                    // 1. Prepara o objeto de serviço para ser enviado
                    var navigationParameter = new Dictionary<string, object>
                        {
                            { "Servico", ServicoSelecionado }
                        };

                    // 2. Pede para o Shell navegar para a rota da página, passando o parâmetro
                    await Shell.Current.GoToAsync(nameof(pgFinalizarAgendamento), navigationParameter);
                }

                // Exemplo de navegação para a próxima tela do agendamento
                // await Shell.Current.GoToAsync($"//proximaPagina?servicoId={idDoServico}");
            }
        }
    }
}
