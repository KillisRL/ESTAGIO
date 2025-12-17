using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BarbeariaMatutosApp.Services;
using UsersDomain.Entidades;
using BarbeariaMatutosApp.Views;

namespace BarbeariaMatutosApp.ViewModels
{
    [QueryProperty(nameof(Servico), "Servico")]
    public partial class FinalizarAgendamentoViewModel : ObservableObject
    {
        Agendamento agendamento = new Agendamento();

        private readonly ApiServices _apiService;

        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private int idUsuario;

        [ObservableProperty]
        private Servicos servico; // Propriedade que vai receber o serviço da tela anterior

        [ObservableProperty]
        private ObservableCollection<Barbeiro> barbeiros;

        [ObservableProperty]
        private Barbeiro barbeiroSelecionado;

        [ObservableProperty]
        private DateTime dataSelecionada;

        [ObservableProperty]
        private TimeSpan horaSelecionada;

        [ObservableProperty]
        private bool isBusy;

        public FinalizarAgendamentoViewModel(ApiServices apiService)
        {
            CarregarDadosUsuario();
            _apiService = apiService;
            Barbeiros = new ObservableCollection<Barbeiro>();
            DataSelecionada = DateTime.Now; // Inicia com a data atual
            HoraSelecionada = DateTime.Now.TimeOfDay; // Inicia com a hora atual
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
        private async Task LoadBarbeirosAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var barbeirosDaApi = await _apiService.GetBarbeirosAsync();
                if (barbeirosDaApi != null)
                {
                    Barbeiros.Clear();
                    foreach (var barbeiro in barbeirosDaApi)
                    {
                        Barbeiros.Add(barbeiro);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"Não foi possível carregar os barbeiros. Erro: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task FinalizarAgendamentoAsync()
        {
            // 1. Validação (seu código já está correto)
            if (Servico is null || BarbeiroSelecionado is null)
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Por favor, selecione um serviço e um barbeiro.", "OK");
                return;
            }

            try
            {
                IsBusy = true; // Ativa o indicador de "carregando"

                // 2. Criar o objeto DTO com os dados selecionados na tela
                var agendamentoRequest = new CriarAgendamentoDTO
                {
                    IdServico = Servico.IdServico,
                    IdBarbeiro = BarbeiroSelecionado.IdBarbeiro,
                    // Combina a data selecionada no DatePicker com a hora do TimePicker
                    DataHora = DataSelecionada.Date + HoraSelecionada,
                    IdSituacao = agendamento.IdSituacao = 1,
                    IDUsuario = SessaoUsuarioService.Usuariologado.IDUsuario
                };
                 
                // 3. Chamar o ApiService para enviar o DTO para a API
                bool sucesso = await _apiService.SalvarAgendamentoAsync(agendamentoRequest);

                // 4. Dar feedback ao usuário e navegar
                if (sucesso)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso!", "Seu agendamento foi confirmado.", "OK");
                    // Volta para a página raiz da aplicação
                    await Shell.Current.GoToAsync(nameof(pgPrincipal));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível completar o agendamento. Por favor, tente novamente mais tarde.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Lida com erros inesperados (ex: falha de conexão)
                await Application.Current.MainPage.DisplayAlert("Erro Crítico", $"Ocorreu um problema de comunicação: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false; // Garante que o indicador de "carregando" sempre pare
            }
        }
    }
}
